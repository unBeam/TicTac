# TicTacToe Test Task

## Минимальную uml диаграмму.

![UML Diagram](Assets/Docs/uml.png)

Диаграмма показывает основной модульный поток: host-сцена вызывает миниигру через `ITicTacToeMiniGameRunner`, runner загружает Addressable-prefab, внутри prefab запускается игровой цикл, после завершения наружу возвращается `TicTacToeMiniGameResult`.

```
flowchart LR
subgraph Host["Host Scene"]
Demo["TicTacToeDemoLauncher"]
RunnerApi["ITicTacToeMiniGameRunner"]
end

    subgraph Infra["Module Loading"]
        Runner["AddressableTicTacToeMiniGameRunner"]
        AssetConfig["TicTacToeMiniGameAssetConfig"]
    end

    subgraph Module["TicTacToe Mini Game Prefab"]
        Root["TicTacToeMiniGameRoot"]
        Presenter["TicTacToeBoardPresenter"]
        Session["TicTacToeMiniGameSession"]
        Result["TicTacToeMiniGameResult"]
        Reward["RewardData"]
    end

    Demo --> RunnerApi
    RunnerApi --> Runner
    Runner --> AssetConfig
    Runner --> Root
    Root --> Presenter
    Presenter --> Session
    Session --> Result
    Result --> Reward
    Runner --> Result
    Result --> Demo
```

## Какие ИИ-агенты использовались.

- **Codex** - основной AI-агент, использовался для поэтапной генерации структуры проекта, каркаса файлов, domain/application слоя, presentation слоя, интеграции runner и demo launcher.

---

## Example Prompts

Ниже приложены 4 промта, использованные в процессе работы.

### 1. Demo / Bootstrap
![Prompt 1](Assets/Docs/promts/1.jpg)

### 2. Presentation слой
![Prompt 2](Assets/Docs/promts/2.jpg)

### 3. Domain / Application слои
![Prompt 3](Assets/Docs/promts/3.jpg)

### 4. Глобальный промт
![Prompt 4](Assets/Docs/promts/4.jpg)

## Что пришлось исправить или переписать вручную.

После проверки сгенерированного решения часть кода и интеграции была доработана вручную:

- DI-конфигурация была разделена на два installer’а:
    - `TicTacToeHostInstaller` для host-сцены
    - `TicTacToeMiniGameInstaller` для внутренних зависимостей prefab-модуля

Это было сделано для более чистой границы между host-сценой и встроенным модулем миниигры.

- вручную добавлена небольшая задержка между ходом игрока и ответом бота
- вручную добавлена небольшая задержка после завершения матча перед закрытием миниигры
- вручную собраны scene/prefab объекты, inspector references и Addressables-конфигурация
- вручную проверен сценарий внешнего запуска и возврата результата во внешний UI

---

## Видео с демонстрацией что все работает. 

[YouTube Demo](https://youtu.be/fgoCYjguOpQ)