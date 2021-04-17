# Lexica

English vocabulary learning software written in .NET Core 5 (C#). Lexica is a console application which can help in
the process of learning English vocabulary.

## How it works

In order to use this application first you have to create a set of words. A words set consists of english words
and their translations in your native language. Sets have to be kept in txt files in the following format:

```
compelling ; nieodparty, pociągający
lunatic ; obłąkany, wariacki
rule of thumb ; praktyczna zasada
rage, anger ; gniew
criteria ; kryteria
```

In every line there is an entry with English meaning + semicolon + meaning in your native language.

Application offers three modes of learning:

- Spelling - Application plays English pronunciation recording of every word in set and user has to write what has just
heard.
- Only open questions - Application asks open questions about every entry in set. For each entry there are at least two
questions (the number of questions depends on configuration):
  - one question about English meaning,
  - one question about meaning in your native language.
- Full - Application asks closed and open questions about every entry in set. When a closed question is asked then
there are presented four possible answers and user has to choose one of them. Open questions work in the same way as in
the previous mode. For each entry there are at least four questions, two closed questions and two open questions (the
number of questions depends on configuration):
  - one closed question about English meaning,
  - one closed question about meaning in your native language,
  - one open question about English meaning,
  - one open question about meaning in your native language.

Application uses SQLite database for storing information about user's anwers (more information).

Application also uses a configuration file (appsettings.json) in which you can change options connected with running
modes.

## How to use it

The simpliest way is just opening PowerShell in application main directory and using the following command:

```
.\Lexica.CLI.exe run --mode only-open .\Assets\Examples\set_1.txt
```

It opens application in 'only open questions' mode with entries from set_1.txt file, which is in subdirectory of
application main directory.

The next step can be just showing application help. You can do that in the following way:

```
.\Lexica.CLI.exe --help
```

You can also show help for run command:

```
.\Lexica.CLI.exe run --help
```

If you want, you can open a mode with a few sets:

```
.\Lexica.CLI.exe run --mode full C:\Lexica\set_1.txt C:\Lexica\set_2.txt
```

You can also use additional configuration to indicate a directory where you keep sets. In that case you can pass
their paths relatively to that directory. For example, if you indicate a following path in your config:

```
C:\Lexica
```

and in this directory there are following files:

```
set_1.txt
set_2.txt
```

then you can run application in the following way:

```
.\Lexica.CLI.exe run --mode full set_1.txt set_2.txt
```

## Configuration

## Modes

### Spelling

### Only open questions

### Full

## Database