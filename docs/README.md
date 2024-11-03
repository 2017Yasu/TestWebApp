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

# WebAPI プロジェクトの作成

```bash
# cd src/server
dotnet new webapi --name TestWebApp.Server.Api --output TestWebApp.Server.Api --use-controllers
```

実行

```bash
cd TestWebApp.Server.Api
dotnet run
```

```text
$ dotnet run
Building...
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://localhost:5001
info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to shut down.
info: Microsoft.Hosting.Lifetime[0]
      Hosting environment: Development
```

<http://localhost:5001/swagger> にアクセスできる。

ソリューションに作成したプロジェクトを追加する。

```bash
# cd src/server
dotnet sln add TestWebApp.Server.Api
```

現在の構成

```text
.
├── .editorconfig
└── src
    └── server
        ├── .gitignore
        ├── TestWebApp.Server.Api
        │   ├── Controllers
        │   │   └── WeatherForecastController.cs
        │   ├── Program.cs
        │   ├── Properties
        │   │   └── launchSettings.json
        │   ├── TestWebApp.Server.Api.csproj
        │   ├── TestWebApp.Server.Api.http
        │   ├── WeatherForecast.cs
        │   ├── appsettings.Development.json
        │   └── appsettings.json
        └── TestWebApp.Server.sln
```
