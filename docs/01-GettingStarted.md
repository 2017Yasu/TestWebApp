---
title: .Net を使ったウェブサーバー構築クイックスタート
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

# 独自 API を実装する

デフォルトで天気予報の API があるが、それはいったん削除する。
削除するファイルは以下。

* `WeatherForecastController.cs`
* `WeatherForecast.cs`

API を追加するには `Controllers` ディレクトリ配下に一定の決まりを持ってクラスを作成していく。
例として、以下のような API を提供するコントローラを追加する。

* GET /api/system/ping
* POST /api/system/echo

これを提供するコントローラの実装は以下。

```cs
using Microsoft.AspNetCore.Mvc;

namespace TestWebApp.Server.Api.Controllers;

[ApiController, Route("api/system")]
public class SystemController : ControllerBase
{
    [HttpGet("ping")]
    public Task<IActionResult> Ping()
    {
        return Task.FromResult<IActionResult>(Ok(new { Message = "Ok" }));
    }

    [HttpPost("echo")]
    public Task<IActionResult> Echo([FromBody] dynamic body)
    {
        return Task.FromResult<IActionResult>(Ok(body));
    }
}
```

ポイントをまとめると、、、

## `ControllerBase` を継承してクラスに適切な属性をつける

コントローラの実装には、まず `ControllerBase` クラスを継承する。
(この辺りの実装で利用するクラスは大体 `Microsoft.AspNetCore.Mvc` 名前空間のもの)

次に `ApiController` 属性をクラスにつける。

API のパスに `api/system` というようなプレフィックスをつけたい場合には、クラスに対して `Route` 属性をつけて、引数にプレフィックスを指定する。

## `HttpGet` 属性で GET リクエストをハンドルする

API を実装するため `IActionResult` か非同期ならば `Task<IActionResult>` を返すメソッドを追加する。
追加したメソッドに `HttpGet` 属性をつけて、ハンドルしたいパスを引数に指定する。

この例では `ping` と指定し、クラスには `Route("api/system")` 属性を指定しているので、結果的に `api/system/ping` をこのメソッドがハンドルすることになる。

`Ok` というのは `ControllerBase` から継承されたメソッドで、200 のリスポンスを作成してくれる。引数に指定したオブジェクトは JSON に変換される。他にも `NotFound` などがある。

## `HttpPost` 属性で POST リクエストをハンドルする

GET の時と同様にメソッドを追加して、そのメソッドに `HttpPost` 属性をつける。引数にはパスを指定する。

メソッドの引数には期待する型 (ここではなんでも受け入れるように `dynamic` にしている) を指定して `FromBody` 属性をつける。
型には任意のクラスを指定できる。

返し方は GET リクエストのときと同じ。

# API をテストする

`dotnet` で作成したウェブ API にはデフォルトで Swagger が組み込まれている。
なので `dotnet run` で実行した後 `http://localhost:{PORT}/swagger` にアクセスすると作成した API を簡単にテストすることができる。

![Swagger page when implementing system controller](./img/swagger-page-system-api.png)
