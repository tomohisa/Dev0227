# C# + Sekiban + Orleans を使った、AI開発のサンプルリポジトリ
# Sample Repository for AI Development using C# + Sekiban + Orleans

# School Management System / 学校管理システム

A web application for managing students, teachers, and classes built with ASP.NET Core, Blazor, and Sekiban Event Sourcing.

ASP.NET Core、Blazor、Sekiban イベントソーシングを使用して構築された、生徒、教師、クラスを管理するためのウェブアプリケーションです。

## Viewport Scaling Feature / ビューポートスケーリング機能

This application includes a viewport scaling feature that allows you to adjust the size of the user interface to better fit your screen resolution. By default, the UI is scaled to 66% of its original size.

このアプリケーションには、ユーザーインターフェースのサイズを調整して画面解像度に合わせることができるビューポートスケーリング機能が含まれています。デフォルトでは、UIは元のサイズの66%にスケーリングされています。

### How to Toggle Viewport Scaling / ビューポートスケーリングの切り替え方法

There are several ways to enable or disable viewport scaling:

ビューポートスケーリングを有効または無効にするには、いくつかの方法があります：

#### 1. Using Keyboard Shortcut / キーボードショートカットの使用

Press `Ctrl + Alt + S` to toggle viewport scaling on or off.

ビューポートスケーリングのオン/オフを切り替えるには、`Ctrl + Alt + S`を押します。

#### 2. Using JavaScript Console / JavaScriptコンソールの使用

Open the browser's developer console (F12 or Ctrl+Shift+I) and use one of these functions:

ブラウザの開発者コンソール（F12またはCtrl+Shift+I）を開き、次の関数のいずれかを使用します：

```javascript
// Enable scaling
enableViewportScaling();

// Disable scaling
disableViewportScaling();

// Toggle scaling
toggleViewportScaling();

// Check if scaling is enabled
isViewportScalingEnabled();
```

#### 3. Editing the HTML / HTMLの編集

To permanently disable viewport scaling, you can edit the `App.razor` file and comment out or remove the viewport-scale.css link:

ビューポートスケーリングを永続的に無効にするには、`App.razor`ファイルを編集し、viewport-scale.cssリンクをコメントアウトまたは削除します：

```html
<!-- Viewport Scaling - Comment out or remove this line to disable scaling -->
<link rel="stylesheet" href="@Assets["viewport-scale.css"]" />
```

> **Note / 注意:** If you disable viewport scaling by editing the HTML, you'll need to rebuild and restart the application for the changes to take effect.

> **注意:** HTMLを編集してビューポートスケーリングを無効にする場合、変更を有効にするにはアプリケーションを再ビルドして再起動する必要があります。

### Customizing the Scaling Factor / スケーリング係数のカスタマイズ

If you want to use a different scaling factor than 66%, you can edit the `viewport-scale.css` file and change the scale values. See the [viewport scaling help page](SchoolManagement.Web/wwwroot/viewport-scaling-help.html) for detailed instructions.

66%以外のスケーリング係数を使用したい場合は、`viewport-scale.css`ファイルを編集してスケール値を変更できます。詳細な手順については、[ビューポートスケーリングヘルプページ](SchoolManagement.Web/wwwroot/viewport-scaling-help.html)を参照してください。

### Help Documentation / ヘルプドキュメント

For more detailed information about the viewport scaling feature, visit the help page at `/viewport-scaling-help.html` when the application is running.

ビューポートスケーリング機能の詳細については、アプリケーション実行時に`/viewport-scaling-help.html`のヘルプページを参照してください。

## Running the Application / アプリケーションの実行

To run the application:

アプリケーションを実行するには：

```bash
cd SchoolManagement/SchoolManagement.AppHost
dotnet run --launch-profile https
```

The web frontend will be available at https://localhost:7201.

Webフロントエンドは https://localhost:7201 で利用可能になります。

## Project Structure / プロジェクト構造

- **SchoolManagement.Domain**: Contains domain models, events, commands, and queries / ドメインモデル、イベント、コマンド、クエリを含む
- **SchoolManagement.ApiService**: API endpoints for commands and queries / コマンドとクエリのAPIエンドポイント
- **SchoolManagement.Web**: Web frontend with Blazor / Blazorを使用したWebフロントエンド
- **SchoolManagement.AppHost**: Aspire host for orchestrating services / サービスをオーケストレーションするためのAspireホスト
- **SchoolManagement.ServiceDefaults**: Common service configurations / 共通サービス設定

## Next.js Project / Next.js プロジェクト

A forthcoming Next.js project that follows the BFF (Backend for Frontend) pattern, accessing APIs on the server side, is located in the `SchoolManagement/school-management-next` directory, where enhancements and advanced features are being developed to extend the School Management System.

## License / ライセンス

This project is licensed under the Apache License 2.0 - see the [LICENSE](./LICENSE) file for details.

このプロジェクトは Apache License 2.0 の下でライセンスされています。詳細は [LICENSE](./LICENSE) ファイルを参照してください。
