---
title: .Net を使ったウェブサーバー構築手順
tags:
  - dotnet
private: false
organization_url_name: null
slide: false
ignorePublish: false
---

# ソリューション作成

まず以下のようにディレクトリが作成されているとする。

```text
.
└── src
    └── server
```

ルートのディレクトリで以下のコマンドを実行。
`sln` ファイルが `src/server` ディレクトリ内に作成される。

```bash
dotnet new sln --name TestWebApp.Server --output src/server
```

## オプション

`.editorconfig` と `.gitignore` を作成する。

`.editorconfig` はルートディレクトリで以下のコマンドを実行して作成。

```bash
dotnet new editorconfig
```

設定は後でいいように変更するがとりあえず置いておく。

`dotnet` 用の `.gitignore` ファイルを以下のコマンドで作成。
コマンドは `src/server` ディレクトリ内で実行した。

```bash
# cd src/server
dotnet new gitignore
```

現在のディレクトリ構成はこんな感じ。

```text
.
├── .editorconfig
└── src
    ├── client
    └── server
        ├── .gitignore
        └── TestWebApp.Server.sln
```
