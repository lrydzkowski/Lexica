# Lexica - programming project

## Core.dll

Namespace:

- Lexica.Core

Classes:

- Config
  - IConfigSource.cs
  - FileConfigSource.cs
    - public FileConfigSource(string path = null)
    - public string GetContents()
  - AppSettings.cs
    - public AppSettings(IConfigSource configSource)
    - private void Validate()
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
- CoreException
  - AppException.cs
    - string Code
    - string Message
  - AppException\<T\> : AppException.cs
    - T Data
  - WrongConfigException : AppException.cs
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
  - Error.cs
    - string Code
    - string Message
  - Error\<T\> : Error.cs
    - T Data
  - OperationResult.cs
    - bool Result
    - List\<Error\> Errors
  - OperationResult\<T\> : OperationResult.cs
    - T Data

Sample configuration:

- appsettings.json

Configuration schema:

- appsettings.schema.json

## CoreTests (xUnit)

Namespace:

- Lexica.Core.Tests

Classes:

- AppSettingsTests.cs
  [Theory]
  [InlineData("")]
  - public void Init_NotExistedPath_ThrowsAppException(string notExistedPath)
  [Theory]
  [InlineData("")]
  - public void Load_WrongConfiguration_ThrowsWrongConfigException(string configPath)
  [Theory]
  [InlineData("")]
  - public void Get_CorrectConfiguration_ReturnsSettingsObject(string configPath)

## WordsManager.dll

Namespace:

- Lexica.Words

Classes:

- Manager.cs
  - public List\<SetInfo\> GetSetsInfo()
  - public OperationResult\<Set\> GetSet(int setId)
  - public OperationResult\<Set\> GetSet(List\<string\> setsIds)
  - public OperationResult ChangeSetName(string oldFullSetName, string newFullSetName)
  - public OperationResult ChangeNamespaceName(string oldName, string newName)
  - public OperationResult RemoveSet(string fullSetName)
- Importer.cs
  - public static bool Import(string directoryPath)
- WordsException
  - WrongFileStructureException
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
    - public Entry Get(int setId, int id)
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

## WordsManagerIntegrationTests (xUnit)

Namespace:

- Lexica.Words.IntegrationTests

Classes:

- CreatingTests.cs
  [Theory]
  - public void Import_NotExistedDirPath_ThrowsAppException(string notExistedDirPath)
  [Theory]
  - public void Import_WrongFileStructure_WrongFileStructureException(string directoryPath)
  [Theory]
  - public void Import_CorrectDirectoryPath_CorrectImport(string directoryPath)
- SelectingTests.cs
  [Fact]
  - public void GetList_CorrectConditions_GetCorrectList()
  [Theory]
  - public void GetSet_NotExistedSet_ReturnsFalseResult(int notExistedSetId)
  [Theory]
  - public void GetSet_NotExistedSets_ReturnsFalseResult(List\<string\> notExistedSetsIds)
  [Theory]
  - public void GetSet_CorrectSetId_ReturnsTrueResult(int setId)
  [Theory]
  - public void GetSet_CorrectSetsIds_ReturnsTrueResult(List\<string\> setsIds)
- ChangingTests.cs
  [Theory]
  - public void ChangeSetName_WrongCurrentSetName_ReturnsFalseResult(
      string wrongOldFullSetName,
      string newFullSetName
    )
  [Theory]
  - public void ChangeSetName_WrongCharsInNewName_ReturnsFalseResult(
      string oldFullSetName,
      string wrongNewFullSetName
    )
  [Theory]
  - public void ChangeSetName_CorrectNames_ReturnsTrueResult(
      string oldFullSetName,
      string newFullSetName
    )
  [Theory]
  - public void ChangeNamespaceName_WrongCurrentNamespaceName_ReturnsFalseResult(
      string wrongOldName,
      string newName
    )
  [Theory]
  - public void ChangeNamespaceName_WrongCharsInNamespaceName_ReturnsFalseResult(
      string oldName,
      string wrongNewName
    )
  [Theory]
  - public void ChangeNamespaceName_CorrectNames_ReturnsTrueResult(
      string oldName,
      string newName
    )
- RemovingTests.cs
  [Theory]
  - public void RemoveSet_NotExistedName_ReturnsFalseResult(string notExistedFullSetName)
  [Theory]
  - public void RemoveSet_CorrectName_ReturnsTrueResult(string fullSetName)

## SpellingMode.dll

Namespace:

- Lexica.Spelling

Classes:

- Manager.cs
  - public Manager(string setId, Config.Models.Spelling cfg)
  - public Manager(List\<string\> setsIds, Config.Models.Spelling cfg)
  - public Lexica.Core.Models.Question GetQuestion()
  - public Lexica.Core.Models.AnswerResult VerifyAnswer(string answer)
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

## SpellingModeIntegrationTests (xUnit)

Namespace:

- Lexica.Spelling.IntegrationTests

Classes:

- ManagerTests.cs
  [Theory]
  - public void Init_NotExistedSetId_ThrowsAppException(
      string notExistedSetId,
      Config.Models.Spelling cfg
    )
  [Theory]
  - public void Init_NotExistedSetsIds_ThrowsAppException(
      List\<string\> notExistedSetsIds,
      Config.Models.Spelling cfg
    )
  )
  [Theory]
  - public void VerifyAnswer_WrongAnswer_ReturnsFalseAnswerResult(string wrongAnswer)
  [Theory]
  - public void VerifyAnswer_CorrectAnswer_ReturnsTrueAnswerResult(string answer)
  [Fact]
  - public void QuestionsRandomness_CorrectData_QuestionsInRandomOrder()

## LearningMode.dll

Namespace:

- Lexica.Learning

Classes:

- Manager.cs
  - public Manager(string setId, Config.Models.Learning cfg)
  - public Manager(List\<string\> setsIds, Config.Models.Learning cfg)
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

## LearningModeIntegrationTests (xUnit)

Namespace:

- Lexica.Learning.IntegrationTests

Classes:

- MangerTests.cs
  [Theory]
  - public void Init_NotExistedSetId_ThrowsAppException(
      string notExistedSetId,
      Config.Models.Learning cfg
    )
  [Theory]
  - public void Init_NotExistedSetsIds_ThrowsAppException(
      List\<string\> notExistedSetsIds,
      Config.Models.Learning cfg
    )
  )
  [Theory]
  - public void VerifyAnswer_WrongAnswer_ReturnsFalseAnswerResult(string wrongAnswer)
  [Theory]
  - public void VerifyAnswer_CorrectAnswer_ReturnsTrueAnswerResult(string answer)
  [Fact]
  - public void QuestionsOrder_CorrectData_QuestionsInCorrectOrder()
  [Fact]
  - public void QuestionsRandomness_CorrectData_QuestionsInRandomOrder()

## MaintainingMode.dll

Namespace:

- Lexica.Maintaining

Classes:

- Manager.cs
  - public Manager(string setId, Config.Models.Maintaining cfg)
  - public Manager(List\<string\> setsIds, Config.Models.Maintaining cfg)
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

## MaintainingModeIntegrationTests (xUnit)

Namespace:

- Lexica.Maintaining.IntegrationTests

Classes:

- ManagerTests.cs
  [Theory]
  - public void Init_NotExistedSetId_ThrowsAppException(
      string notExistedSetId,
      Config.Models.Maintaining cfg
    )
  [Theory]
  - public void Init_NotExistedSetsIds_ThrowsAppException(
      List\<string\> notExistedSetsIds,
      Config.Models.Maintaining cfg
    )
  )
  [Theory]
  - public void VerifyAnswer_WrongAnswer_ReturnsFalseAnswerResult(string wrongAnswer)
  [Theory]
  - public void VerifyAnswer_CorrectAnswer_ReturnsTrueAnswerResult(string answer)
  [Fact]
  - public void QuestionsRandomness_CorrectData_QuestionsInRandomOrder()

## LexicaCLI

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
  Correct answers: 0  / 20
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
    Correct answers:  5  / 20
    Wrong answers:    15 / 20
  Open questions:
    Correct answers:  10 / 20
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
      - public List\<Option\> Options
      - public List\<Command\> Commands
    - Option.cs
    - Command.cs
  - AppCLIDefinition.cs
    - public AppCLIDefinition(string path = null)
    - private void Validate()
    - public void Load()
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
  