# Lexica (Release Candidate 1)

English vocabulary learning software written in .NET Core 5 (C# console application). Application works only on
Windows 10.

## How it works

In order to use this application first you have to create a set of words (an example set file is available in
[project](https://github.com/lrydzkowski/Lexica/blob/develop/Lexica.CLI/Assets/Examples/set_1.txt). A words set consists
of english words and their translations in your native language. Sets have to be kept in txt files in the following
format:

```
compelling ; nieodparty, pociągający
lunatic ; obłąkany, wariacki
rule of thumb ; praktyczna zasada
rage, anger ; gniew
criteria ; kryteria
```

In every line there is an entry with English meaning + semicolon + meaning in your native language.

Application offers three modes of learning:

- [Spelling](#spelling) - Application plays English pronunciation recording of every word in set and user has to write
what has just heard.
- [Only open questions](#only-open-questions) - Application asks open questions about every entry in set. For each entry
there are at least two questions (the number of questions depends on configuration):
  - at least one question about English meaning,
  - at least one question about meaning in your native language.
- [Full](#full) - Application asks closed and open questions about every entry in set. When a closed question is asked
then there are presented four possible answers and user has to choose one of them. Open questions work in the same way
as in the previous mode. For each entry there are at least four questions, two closed questions and two open questions
(the number of questions depends on configuration):
  - one closed question about English meaning,
  - one closed question about meaning in your native language,
  - at least one open question about English meaning,
  - at least one open question about meaning in your native language.

Application uses SQLite database for storing information about user's anwers ([more information](#database)). In the
future this information will be used for statistics and for creating sets from entries which make an user the biggest
troubles.

Application also uses a configuration file (appsettings.json) in which you can change options connected with running
modes ([more information](#configuration)).

## How to use it

The simpliest way is just opening PowerShell in application main folder and using the following command:

```
.\Lexica.CLI.exe run --mode only-open .\Assets\Examples\set_1.txt
```

It opens Lexica in 'only open questions' mode with entries from set_1.txt file, which is in subfolder of the
application main folder.

The next step can be just showing help. You can do that in the following way:

```
.\Lexica.CLI.exe --help
```

You can also show help for 'run' command:

```
.\Lexica.CLI.exe run --help
```

If you want, you can open a mode with a few sets:

```
.\Lexica.CLI.exe run --mode full C:\Lexica\set_1.txt C:\Lexica\set_2.txt
```

You can also use additional configuration to indicate a folder where you keep sets. In that case you can pass
their paths relatively to that folder. For example, if you indicate a following path in your config:

```
C:\Lexica
```

and in this folder there are following files:

```
set_1.txt
set_2.txt
```

then you can run application in the following way:

```
.\Lexica.CLI.exe run --mode full set_1.txt set_2.txt
```

## Configuration

Lexica uses configuration stored in appsettings.json file. Here is presented sample configuration with
comments about available options:

```
{
  // A folder with sets. If you indicate this option, then you can run Lexica with paths to sets which are
  // relative to this folder (more information in 'How to use it' chapter). This part is optional.
  "Words": {
    "SetsDirectoryPath": "C:/Lexica"
  },
  // Modes configuration. This part is required.
  "Learning": {
    // Number of open questions about English meaning and meaning in your native language. So if you set this value to
    // 3 then in 'full' and 'only open' modes for each entry there would be 6 questions. In the 'spelling' mode there
    // would be only 3 questions because in this mode there are only questions about English spelling. This option is
    // required.
    "NumOfOpenQuestions": 3,
    // Should mode be reset after the first mistake? This option is required.
    "ResetAfterMistake": false,
    // Should pronunciation be played in 'full' and 'only-open' modes? This part is required.
    "PlayPronuncation": {
      // A pronunciation record is played before answers to questions about English meaning. This option is required.
      "BeforeAnswer": false,
      // A pronunciation record is played after answers to questions about meaning in your native language. This option
      // is required.
      "AfterAnswer": false
    },
    // Should Lexica save modes logs? These logs are saved in .\Logs\Mode.logs file. This option is required.
    "SaveDebugLogs": false
  },
  // Configuration connected with getting a pronunciation from a web dictionary (it is up to user which dictionary will
  // be used). You have to keep in mind that choosing any web dictionary should take into consideration legal
  // restrictions connected with copyright. This part is optional.
  "PronunciationApi": {
    "WebDictionary": {
      // A web dictionary host.
      "Host": "",
      // An url path to a word page (for example /dict/en/{word}, where {word} is automatically replaced with a word
      // for which should be played a pronuciation).
      "UrlPath": "",
      // A directory path to which mp3 files with pronunciation will be downloaded.
      "DownloadDirectoryPath": ""
    }
  }
}
```

## Modes

### Spelling

#### Main goal of this mode

A main goal of this mode is to learn how to spell words and what is their pronunciation. This is the first phase
of learning new English words.

#### How it works

- Each entry from given sets has its own counter which at the beginning is equal 0.
- Entries in given sets are randomized.
- For each entry an English pronunciation is played and user has to write the particual word.
- After giving a correct answer:
  - A translation is showed.
  - A counter for an entry is icremented by 1. If a counter for the particular entry reaches a number stored in
  Learning.NumOfOpenQuestions configuration option then a question about this entry is not asked again.
- After giving an incorrect answer:
  - A correct answer is showed.
  - A translation is showed.
  - A counter for an entry is set to 0. If Learning.ResetAfterMistake configuration option equals true then counter
  for every entry is set to 0.
- After playing English pronunciation for all entries the following condition is checked:
  - If still there are entries with a counter lower than a number stored in Learning.NumOfOpenQuestions configuration
option then we go back to the first point.
  - Otherwise the mode is over.

### Full

#### Main goal of this mode

A main goal of this mode is to learn words meaning and remember them. This is the second phase of learning new English
words.

#### How it works

- Each entry from given sets has 4 counters which at the beginning are equal 0:
  - A counter for closed questions about English meaning. For each entry a user has to reach this counter equals 1.
  - A counter for closed questions about your native language meaning. For each entry a user has to reach this
  counter equals 1.
  - A counter for open questions about English meaning. For each entry a user has to reach this counter equals a
  number stored in Learning.NumOfOpenQuestions configuration option.
  - A counter for closed questions about your native language meaning.For each entry a user has to reach this counter
  equals a number stored in Learning.NumOfOpenQuestions configuration option.
- Entries in given sets are randomized and 7 first entries are taken. For each of them there is asked a question.
- For each randomly selected entry the following conditions are checked in order to ask a question to user:
  - If there is at least one not reached closed question counter then a closed question is asked. If both counters
  aren't reached then the type of closed question is randomly selected (question about English meaning or question about
  your native language meaning). If one counter isn't reached then the question type of this counter is asked.
  - Otherwise if there is at least one not reached open question counter then an open question is asked. If both
  counters aren't reached then the type of open question is randomly selected (question about English meaning or
  question about your native language meaning). If one counter isn't reached then the question type of this counter is 
  asked.
  - Otherwise an entry is omitted and conditions for next entry are analyzed.
- After giving a correct answer:
  - The particular counter is incremented by 1.
- After giving a wrong answer:
  - The particual counter is set to 0.
  - A correct answer is showed.
- After analyzing 7 entries, again all entries in sets are randomized and first 8 entries are analyzed.
- In order to end this mode a user has to reached all counters.

### Only open questions

#### Main goal of this mode

The goal of this mode is providing the last phase of learning and memory maintaining.

#### How it works

- Each entry from given sets has 2 counters which at the beginning are equal 0:
  - A counter for open questions about English meaning. For each entry a user has to reach this counter equals a
  number stored in Learning.NumOfOpenQuestions configuration option.
  - A counter for closed questions about your native language meaning.For each entry a user has to reach this counter
  equals a number stored in Learning.NumOfOpenQuestions configuration option.
- Entries in given sets are randomized.
- For each randomly selected entry the following conditions are checked in order to ask a question to user:
  - If there is at least one not reached open question counter then an open question is asked. If both
  counters aren't reached then the type of open question is randomly selected (question about English meaning or
  question about your native language meaning). If one counter isn't reached then the question type of this counter is
  asked.
  - Otherwise an entry is omitted and conditions for next entry are analyzed.
- After giving a correct answer:
  - The particular counter is incremented by 1.
- After giving a wrong answer:
  - The particual counter is set to 0.
  - A correct answer is showed.
- After asking questions for all entries the following condition is checked:
  - If still there are entries with a counter lower than a number stored in Learning.NumOfOpenQuestions configuration
option then we go back to point 1.
  - Otherwise the mode is over.

## Technicalities

## Database

Lexica save into SQLite database information about each user's answer. Database is stored in lexica.db file which can
be found in the main application folder.

There is only one table in database:

Table name: **answer**
Columns with their description:

- answer_id - Automatically setting answer id.
- folder_path - Path to folder where a set file was during giving the answer.
- set_file_name - A set file name.
- mode - Application mode.
- question - Asked question.
- question_type - Question type.
- answer - Given answer.
- proper_answers - What was the proper answer to a given question.
- is_correct - A bool value which indicates if an answer was correct.
