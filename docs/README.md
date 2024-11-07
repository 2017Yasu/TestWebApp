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

# 静的ファイルをサーブする

以下のコードを `Program.cs` ファイルに追加。

```diff
 app.UseHttpsRedirection();

+app.UseStaticFiles();

 app.UseAuthorization();
```

これにより `wwwroot` ディレクトリ内のファイルを `localhost:{PORT}/path/to/file.html` などのアドレスで取得することができる。

一方で `localhost:{PORT}/` などのファイルを直接指定しないアドレスでは何も返してこない。この時に `index.html` などのデフォルトのファイルを返すようにするには以下のコードを追記する。

```diff
 app.UseHttpsRedirection();

+app.UseDefaultFiles();
 app.UseStaticFiles();

 app.UseAuthorization();
```

試しに以下のようなファイルを `wwwroot/index.html` として作成して実行すると `localhost:{PORT}` で作成したページを参照することができる。

```html
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Test</title>
</head>
<body>
    <h1>Test</h1>
</body>
</html>
```

# クライアントをフレームワークを使って実装する

ここではクライアントファイルを [Next.js](https://nextjs.org/) を使って構築して、サーバーと繋げてみます。

node はインストールされているものとする。

以下のコマンドを `src` ディレクトリで実行して Next.js プロジェクトを作成する。

```bash
npx create-next-app@latest
```

ここではプロジェクト名を `client` ととした。それ以外はデフォルト。以下実行例。

```text
What is your project named? client
Would you like to use TypeScript? No / Yes <= Yes
Would you like to use ESLint? No / Yes <= Yes
Would you like to use Tailwind CSS? No / Yes <= No
Would you like your code inside a `src/` directory? No / Yes <= Yes
Would you like to use App Router? (recommended) No / Yes <= Yes
Would you like to use Turbopack for `next dev`?  No / Yes <= No
Would you like to customize the import alias (`@/*` by default)? No / Yes <= No
What import alias would you like configured? @/*
```

現在のフォルダ構成は以下のような感じ。

```text
.
└── src
    ├── client
    │   ├── README.md
    │   ├── next-env.d.ts
    │   ├── next.config.ts
    │   ├── package-lock.json
    │   ├── package.json
    │   ├── public
    │   │   ├── file.svg
    │   │   ├── globe.svg
    │   │   ├── next.svg
    │   │   ├── vercel.svg
    │   │   └── window.svg
    │   ├── src
    │   │   └── app
    │   │       ├── favicon.ico
    │   │       ├── fonts
    │   │       │   ├── GeistMonoVF.woff
    │   │       │   └── GeistVF.woff
    │   │       ├── globals.css
    │   │       ├── layout.tsx
    │   │       ├── page.module.css
    │   │       └── page.tsx
    │   └── tsconfig.json
    └── server
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
        │   ├── appsettings.json
        │   └── wwwroot
        │       └── index.html
        └── TestWebApp.Server.sln
```

Next.js の場合は静的ページを生成するためには以下のように `next.config.ts` を修正する必要がある ([参考](https://nextjs.org/docs/app/building-your-application/deploying/static-exports))。

```ts
import type { NextConfig } from "next";

const nextConfig: NextConfig = {
  output: 'export'
  /* config options here */
};

export default nextConfig;
```

以下のコマンドでビルドする。

```bash
npm run build
```

すると `out` ディレクトリが `client` ディレクトリ下にできて、その中に `html` や `js` ファイルなどが生成される。これを `server/TestWebApp.Server.Api/wwwroot` ディレクトにコピーすればサーバー側でサーブできるようになる。

## 開発のための設定

### サーバー側

開発中何度もコピーするでは面倒なので、ここではシンボリックを使って `server/TestWebApp.Server.Api/wwwroot` と `client/out` ディレクトリを繋げる。

`server/TestWebApp.Server.Api` ディレクトリに移動して、ひとまず `wwwroot` ディレクトリを削除する。

```bash
rm -rf wwwroot
```

以下のコマンドでシンボリックリンクを作成する。

```bash
ln -s ../../client/out wwwroot
```

すると `wwwroot` という名前で `../../client/out` にアクセスすることができる。

この状態で `dotnet run` を実行して `http://localhost:{PORT}` にアクセスすると Next.js のスタートページが表示される。

※ Windows で同様のシンボリックリンクを作成するには以下のコマンドを管理者権限で実行する ([参考](https://dev.classmethod.jp/articles/make_windows_symbolic_link/))。

```powershell
mklink /D wwwroot ../../client/out
```
