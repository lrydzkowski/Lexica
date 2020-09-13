# Lexica - programming project

## Core.dll

Namespace:

- Lexica.Core

Classes:

- Config
  - AppSettings.cs
    - public AppSettings(string path = null)
    - private Validate()
    - public Load()
    - public Config.Models.Settings Get()
  - Models:
    - Settings.cs
    - Database.cs
    - Words.cs
    - Spelling.cs
    - Learning.cs
    - Maintaining.cs
- Data
  - LexicaContext.cs
- Models:
  - QuestionTypeEnum.cs
    - Open
    - Closed
  - Question.cs
    - string Content
    - QuestionTypeEnum Type
    - List\<string\> PossibleAnswers
  - AnswerResult.cs
    - bool Result
    - List\<string\> Answers

Sample configuration:

- appsettings.json

Configuration schema:

- appsettings.schema.json

## CoreTests

What do I want to test?

- Config file validation.

## WordsManager.dll

Namespace:

- Lexica.Words

Classes:

- Manager.cs
  - public List\<SetInfo\> GetSetsInfo()
  - public Set GetSet(int setId)
  - public Set GetSet(List\<string\> setIds)
  - public bool ChangeSetName(string oldFullSetName, string newFullSetName)
  - public bool ChangeNamespaceName(string oldName, string newName)
  - public bool RemoveSet(string fullSetName)
- Importer.cs
  - public static bool Import(string directoryPath)
- Models:
  - Entry.cs
    - string Id
    - List\<string\> Words
    - List\<string\> Translations
  - Set.cs
    - string Id
    - string Namespace
    - string Name
    - List\<Entry\> Entries
    - public Randomize()
    - public Entry Get(int id)
    - public Entry GetNext()
  - SetInfo.cs
    - long Id
    - string Namespace
    - string Name
- Data:
  - Models:
    - SetTable.cs
      - public long Id
      - public string Namespace - MaxLength(400)
      - public string Name - MaxLength(100)
    - EntryTable.cs
      - public long RecId
      - public long SetId
      - public int EntryId
      - public string Word - MaxLength(50)
      - public string Translation - MaxLength(50)

## SpellingMode.dll

Namespace:

- Lexica.Spelling

Classes:

- Manager.cs
  - public Manager(string setId, Config.Models.Spelling cfg)
  - public Manager(List\<string\> setIds, Config.Models.Spelling cfg)
  - public Lexica.Core.Models.Question GetQuestion()
  - public Lexica.Core.Models.AnswerResult VerifyAnswer(string input)
- Data
  - Models:
    - SpellingHistory
      - public long Id
      - public long SetId
      - public int EntryId
      - public bool IsWord
      - public bool IsTranslation
      - public long NumOfCorrectAnswers
      - public long NumOfMistakes

## LearningMode.dll

Namespace:

- Lexica.Learning

Classes:

- Manager.cs
  - public Manager(string setId, Config.Models.Learning cfg)
  - public Manager(List\<string\> setIds, Config.Models.Learning cfg)
  - public Lexica.Core.Models.Question GetQuestion()
  - public Lexica.Core.Models.AnswerResult VerifyAnswer(string input)
- Data:
  - Models:
    - LearningHistory
      - public long Id
      - public long SetId
      - public int EntryId
      - public bool IsWord
      - public bool IsTranslation
      - public long NumOfCorrectAnswersOpenQuestion
      - public long NumOfMistakesOpenQuestion
      - public long NumOfCorrectAnswersClosedQuestion
      - public long NumOfMistakesClosedQuestion

## MaintainingMode.dll

Namespace:

- Lexica.Maintaining

Classes:

- Manager.cs
  - public Manager(string setId, Config.Models.Maintaining cfg)
  - public Manager(List\<string\> setIds, Config.Models.Maintaining cfg)
  - public Lexica.Core.Models.Question GetQuestion()
  - public Lexica.Core.Models.AnswerResult VerifyAswer(string input)
- Data:
  - Models:
    - MaintainingHistory
      - public long Id
      - public long SetId
      - public int EntryId
      - public bool IsWord
      - public bool IsTranslation
      - public long NumOfCorrectAnswers
      - public long NumOfMistakes

## CLI

CLI Interface:

```code
PS C:\> Lexica
PS C:\> Lexica -h
PS C:\> Lexica --help

Info:   English learning application.
Usage:  Lexica [OPTIONS] [COMMANDS]

Options:
  -h | --help       Show help.
  -v | --version    Show info about version.

Commands:
  ls  | list        Show list of sets.
  wls | wlist       List of words in set or sets.
  r   | run         Run application.
```

```code
PS C:\> Lexica -v
PS C:\> Lexica --version

Lexica 1.0.0.0 (CLI) (build: 202009010)
Copyright (c) Łukasz Rydzkowski
```

```code
PS C:\> Lexica ls
PS C:\> Lexica list
  
Id   |  FullName
-----|--------------------------------------
1    |  \Others\Other - 1
2    |  \Others\Other - 2
3    |  \Others\Other - 3
4    |  \Direct Method - Book 7\Unit 1 - 2
-----|--------------------------------------  
```

```code
PS C:\> Lexica wls
PS C:\> Lexica wlist
PS C:\> Lexica wls -h
PS C:\> Lexica wls --help
PS C:\> Lexica wlist -h
PS C:\> Lexica wlist --help

Info:     Show list of words in selected sets.
Usage:    Lexica wlist [OPTIONS]

Options:
  -s "<set_full_name>" | --set "<set_full_name>"  Select a set from which words should be presented. This
                                                  option can be repeated many times in order to show
                                                  words from more than one set.
```

```code
PS C:\> Lexica wls -s "\Others\Other - 1"
PS C:\> Lexica wlist -set "\Others\Other - 1"

\Others\Other - 1
-------------------------------------------
  incomparably           | nieporównywalnie
  steer                  | sterować
  plaything              | zabawka
  crossing, exceeding    | przekroczenie
  ticket, fine           | mandat
-------------------------------------------
```

```code
PS C:\> Lexica r
PS C:\> Lexica run
PS C:\> Lexica r -h
PS C:\> Lexica r --help
PS C:\> Lexica run -h
PS C:\> Lexica run --help

Info:   Run learning mode.
Usage:  Lexica run [OPTIONS]

Options:
  -m <ModeName> | -- mode <ModeName>     Run mode.
```

```code
PS C:\> Lexica run -m Spelling

(Ctrl + p) Play pronunciation again; (Enter) Answer; (Ctrl + r) Restart; (Ctrl + c) Close;
Progress: 0/20

#

Summary:
  Proper answers:  0  / 20
  Wrong answers:   20 / 20
```

```code
PS C:\> Lexica run -m Learning

(Ctrl + p) Play pronunciation; ([1-4]) Choose answer; (Ctrl + r) Restart; (Ctrl + c) Close;
  
  mandat
  -------------------------
  1: intimidation
  2: plathing
  3: messiah
  4: ticket, fine

(Ctrl + p) Play pronunciation; (Enter) Answer; (Ctrl + f) Override; (Ctrl + f) ; (Ctrl + r) Restart; (Ctrl + c) Close;

  mandat
  -------------------------
  #

Summary:
  Closed questions:
    Proper answers:   5  / 20
    Wrong answers:    15 / 20
  Open questions:
    Proper answers:   10 / 20
    Wrong answers:    10 / 20
```

```code
PS C:\> Lexica run -m Maintaining

(Ctrl + p) Play pronunciation; (Enter) Answer; (Ctrl + f) Override; (Ctrl + r) Restart; (Ctrol + c) Close;

  mandat
  -------------------------
  #
```

Namespace:

Lexica.CLI

Classes:

- Map
  - Models
    - CLIDefinition.cs
    - Option.cs
    - Command.cs
  - AppCLIDefinition.cs
    - public AppCLIDefinition(string path = null)
    - private Validate()
    - public Load()
    - public Map.Models.CLIDefinition Get()
- Services
  - Help.cs
  - Version.cs
  - List.cs
  - SpellingMode.cs
  - LearningMode.cs
  - MaintainingMode.cs
- Cmd.cs
  - private Map.Models.CLIDefinition Args
  - public void ProcessArguments(string[] args)
  - public bool ValidateAgainstMap(Map.Models.CLIDefinition cliDefinition)
  - public void Run()
  