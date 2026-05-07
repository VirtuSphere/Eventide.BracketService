# BracketService - Архитектура и Теория

## Описание Сервиса

**BracketService** - это микросервис, отвечающий за управление турнирными кронштейнами (brackets) в системе организации турниров. Его основная задача - генерировать, отслеживать и управлять состоянием турнирных сеток для различных форматов соревнований.

### Основной Функционал

- **Генерация кронштейнов** - создание турнирной сетки на основе списка участников
- **Управление раундами** - отслеживание прогресса раундов от первого до финала
- **Учет матчей** - управление отдельными поединками и их результатами
- **Продвижение победителей** - автоматическое создание следующих раундов на основе результатов текущих матчей

---

## Архитектура Domain Layer

### Clean Architecture - Domain Driven Design

```
Domain Layer (BracketService.Domain)
├── Entities (Bracket, BracketMatch, BracketRound)
├── Enums (BracketStatus, BracketType, MatchStatus)
├── Exceptions (специфичные исключения для каждого сценария)
├── Base (Entity<T> - базовый класс для всех сущностей)
└── Repositories Abstractions (IBracketRepository)

Value Objects Layer (BracketService.ValueObjects)
├── TotalRounds (ValueObject для общего количества раундов)
├── CurrentRound (ValueObject для текущего раунда)
├── Validators (TotalRoundsValidator, CurrentRoundValidator)
└── Exceptions (специфичные исключения валидации)
```

### Ключевые Принципы Domain Layer

1. **Encapsulation** - все внутренние состояния защищены, изменяются только через методы
   - `private readonly List<BracketRound> _rounds` - коллекция раундов скрыта
   - `public IReadOnlyCollection<BracketRound> Rounds` - безопасный доступ на чтение

2. **Validation** - валидация в конструкторах через специфичные исключения
   ```csharp
   TournamentId = tournamentId != Guid.Empty 
       ? tournamentId 
       : throw new ArgumentNullValueException(nameof(tournamentId));
   ```

3. **Value Objects** - использование ValueObjects вместо примитивных типов
   - `TotalRounds` вместо `int`
   - `CurrentRound` вместо `int`
   - Каждый ValueObject проходит валидацию через IValidator

4. **Specific Exceptions** - каждый сценарий ошибки имеет свое исключение
   - `BracketCompletedException` - кронштейн завершен
   - `MatchAlreadyCompletedException` - матч уже сыгран
   - `MatchInvalidWinnerException` - неправильный победитель
   - `BracketParticipantsInsufficientException` - мало участников

5. **Business Logic** - вся бизнес-логика находится в Domain Layer
   ```csharp
   public void AdvanceWinner(Guid matchId, Guid winnerId)
   {
       // Полная логика продвижения:
       // 1. Проверка завершения кронштейна
       // 2. Поиск матча
       // 3. Установка победителя
       // 4. Проверка завершения раунда
       // 5. Автоматическое создание следующего раунда
   }
   ```

---

## Связь с Другими Сервисами через gRPC

### Архитектура Микросервисов

```
┌─────────────────────────────────────┐
│      API Gateway / Client            │
└──────────────┬──────────────────────┘
               │
    ┌──────────┼──────────┐
    │          │          │
    v          v          v
┌─────────┐ ┌──────────┐ ┌─────────┐
│ Tournament│  Bracket  │ Organizer│
│ Service   │ Service   │ Service  │
└─────────┘ └──────────┘ └─────────┘
    │          │          │
    └──────────┼──────────┘
               │
       gRPC Synchronous Communication
```

### Интеграция BracketService

**BracketService как клиент:**
- Может обращаться к `TournamentService` для получения информации о турнире
- Может обращаться к `OrganizerService` для валидации владельца турнира

**BracketService как сервер:**
- Предоставляет gRPC методы для других сервисов:
  ```protobuf
  service BracketService {
    rpc GenerateBracket(GenerateBracketRequest) returns (GenerateBracketResponse);
    rpc AdvanceWinner(AdvanceWinnerRequest) returns (AdvanceWinnerResponse);
    rpc GetBracketStatus(GetBracketStatusRequest) returns (BracketStatusResponse);
  }
  ```

### Сценарии Взаимодействия

**Сценарий 1: Создание кронштейна**
```
TournamentService (имеет информацию об участниках)
    ↓ (gRPC call)
BracketService.GenerateBracket()
    ↓
Domain Layer генерирует Bracket
    ↓
Repository сохраняет в БД
    ↓ (ответ)
TournamentService получает BracketId
```

**Сценарий 2: Отчет о результате матча**
```
MatchResultService (внешний)
    ↓ (gRPC call)
BracketService.AdvanceWinner(matchId, winnerId)
    ↓
Domain Layer проверяет валидность
    ↓
Если раунд завершен → создает следующий раунд
    ↓
Repository обновляет состояние
    ↓ (events - асинхронно)
EventBus → TournamentService, NotificationService...
```

---

## Состояние Domain Layer - Все ли Нормально?

### ✅ ЧТО СДЕЛАНО ПРАВИЛЬНО

1. **Правильное разделение ответственности**
   - Domain содержит ТОЛЬКО бизнес-логику
   - Нет зависимостей от инфраструктуры (БД, HTTP, gRPC)
   - Готов для тестирования unit-тестами без моков

2. **Полная инкапсуляция состояния**
   - Коллекции скрыты за IReadOnlyCollection
   - Все изменения идут через методы сущностей
   - Нет setter'ов на важные свойства

3. **Специфичные исключения**
   - Каждый сценарий ошибки имеет свой тип
   - Легко ловить и обрабатывать в Application Layer
   - Самодокументируемый код

4. **Value Objects с валидацией**
   - CurrentRound и TotalRounds - не просто int
   - Валидация на уровне конструктора
   - Невозможно создать невалидное значение

5. **Бизнес-логика продвижения**
   - AdvanceWinner содержит полную логику
   - Автоматическое создание следующих раундов
   - Обработка финала (завершение турнира)

### ⚠️ ЧТО НУЖНО ДОДЕЛАТЬ ДЛЯ PRODUCTION

1. **Добавить Domain Events**
   ```csharp
   public class Bracket : Entity<Guid>
   {
       private readonly List<DomainEvent> _domainEvents = new();
       public IReadOnlyCollection<DomainEvent> DomainEvents => _domainEvents.AsReadOnly();
       
       private void RaiseDomainEvent(DomainEvent @event) => _domainEvents.Add(@event);
       
       public void AdvanceWinner(...)
       {
           // ... логика ...
           RaiseDomainEvent(new WinnerAdvancedEvent(...));
           if (IsRoundCompleted)
               RaiseDomainEvent(new RoundCompletedEvent(...));
           if (Status == BracketStatus.Completed)
               RaiseDomainEvent(new BracketCompletedEvent(...));
       }
   }
   ```
   **Зачем:** Позволяет другим сервисам реагировать на события (NotificationService, StatisticsService)

2. **Добавить Repository Pattern правильно**
   ```csharp
   // В Domain:
   public interface IBracketRepository
   {
       Task<Bracket?> GetByIdAsync(Guid id);
       Task SaveAsync(Bracket bracket);
   }
   
   // В Application Layer (через DI):
   public class AdvanceWinnerCommand
   {
       public async Task Execute(Guid bracketId, Guid matchId, Guid winnerId)
       {
           var bracket = await _repository.GetByIdAsync(bracketId);
           bracket.AdvanceWinner(matchId, winnerId);
           await _repository.SaveAsync(bracket);
       }
   }
   ```

3. **Добавить Unit of Work для транзакций**
   - При сохранении нескольких сущностей
   - Гарантирует ACID свойства

4. **Specification Pattern для сложных запросов**
   ```csharp
   var completedBrackets = await _repository.FindAsync(
       new BracketsCompletedInDateRangeSpec(startDate, endDate)
   );
   ```

5. **Aggregate Root правильно**
   - Bracket = Aggregate Root
   - BracketRound и BracketMatch = дочерние сущности, доступны ТОЛЬКО через Bracket
   - Нельзя загружать BracketMatch напрямую - ТОЛЬКО через Bracket

### 📋 ИНТЕГРАЦИЯ С gRPC

**Application Layer (обслуживает gRPC контракты):**
```csharp
// gRPC Service
public class BracketGrpcService : BracketService.BracketServiceBase
{
    private readonly IMediator _mediator;
    
    public override async Task<GenerateBracketResponse> GenerateBracket(
        GenerateBracketRequest request, 
        ServerCallContext context)
    {
        try
        {
            var command = new GenerateBracketCommand(
                request.TournamentId, 
                request.ParticipantIds.ToList());
            
            var result = await _mediator.Send(command);
            return new GenerateBracketResponse { BracketId = result.Id.ToString() };
        }
        catch (BracketParticipantsInsufficientException ex)
        {
            throw new RpcException(new Status(
                StatusCode.InvalidArgument, 
                ex.Message));
        }
        catch (BracketCompletedException ex)
        {
            throw new RpcException(new Status(
                StatusCode.FailedPrecondition, 
                ex.Message));
        }
    }
}
```

**Domain остается чистым - нет знаний о gRPC!**

---

## Вывод

### Domain Layer - ✅ ГОТОВ К PRODUCTION

- Правильно организована архитектура
- Все бизнес-правила инкапсулированы
- Легко тестировать и расширять
- Нет внешних зависимостей

### Для полной готовности требуется

- Domain Events для асинхронной коммуникации
- Application Layer с CommandHandlers
- Infrastructure Layer с Repository имплементацией
- gRPC контракты и Service имплементация

**Рекомендация:** BracketService готов для переиспользования в других проектах, так как Domain Layer полностью независим от конкретной технологии (gRPC, REST, Events).
