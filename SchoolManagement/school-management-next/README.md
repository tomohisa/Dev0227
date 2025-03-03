# School Management System - Next.js Frontend / 学校管理システム - Next.jsフロントエンド

This is a Next.js implementation of the School Management System frontend, built with Next.js and shadcn/ui. It provides a modern, responsive interface for managing students, teachers, and classes.

これは、Next.jsとshadcn/uiで構築された学校管理システムフロントエンドのNext.js実装です。生徒、教師、クラスを管理するための現代的でレスポンシブなインターフェースを提供します。

## Features / 機能

- **Student Management / 生徒管理**: Register, update, and delete student records. Assign students to classes. / 生徒の記録の登録、更新、削除。生徒をクラスに割り当て。
- **Teacher Management / 教師管理**: Register, update, and delete teacher records. Assign teachers to classes. / 教師の記録の登録、更新、削除。教師をクラスに割り当て。
- **Class Management / クラス管理**: Create, update, and delete class records. Assign teachers and students to classes. / クラスの記録の作成、更新、削除。教師と生徒をクラスに割り当て。
- **Modern UI / モダンUI**: Built with Next.js and shadcn/ui for a clean, responsive interface. / Next.jsとshadcn/uiで構築された、クリーンでレスポンシブなインターフェース。
- **Dark Mode / ダークモード**: Supports light and dark mode with theme switching. / テーマ切り替えによるライトモードとダークモードのサポート。
- **Responsive Design / レスポンシブデザイン**: Works on desktop and mobile devices. / デスクトップとモバイルデバイスで動作。

## Technologies Used / 使用技術

- **Next.js**: React framework for server-rendered applications / サーバーレンダリングアプリケーション用のReactフレームワーク
- **React**: JavaScript library for building user interfaces / ユーザーインターフェースを構築するためのJavaScriptライブラリ
- **TypeScript**: Typed JavaScript for better developer experience / より良い開発者体験のための型付きJavaScript
- **Tailwind CSS**: Utility-first CSS framework / ユーティリティファーストのCSSフレームワーク
- **shadcn/ui**: Reusable UI components built with Radix UI and Tailwind CSS / Radix UIとTailwind CSSで構築された再利用可能なUIコンポーネント
- **Axios**: Promise-based HTTP client for API requests / APIリクエスト用のPromiseベースのHTTPクライアント
- **Tanstack Table**: Headless UI for building powerful tables and datagrids / 強力なテーブルとデータグリッドを構築するためのヘッドレスUI

## Getting Started / はじめに

### Prerequisites / 前提条件

- Node.js 18.x or later / Node.js 18.x以降
- npm or yarn / npmまたはyarn

### Installation / インストール

1. Clone the repository / リポジトリをクローン
2. Install dependencies / 依存関係をインストール:

```bash
cd school-management-next
npm install
```

3. Create a `.env.local` file with the following content / 以下の内容で`.env.local`ファイルを作成:

```
NEXT_PUBLIC_API_URL=https://localhost:7370
```

4. Start the development server / 開発サーバーを起動:

```bash
npm run dev
```

5. Open [http://localhost:3000](http://localhost:3000) in your browser. / ブラウザで[http://localhost:3000](http://localhost:3000)を開きます。

## API Integration / API連携

This frontend connects to the School Management API, which is built with ASP.NET Core and uses the Sekiban event sourcing framework. The API provides endpoints for managing students, teachers, and classes.

このフロントエンドは、ASP.NET Coreで構築され、Sekibanイベントソーシングフレームワークを使用する学校管理APIに接続します。APIは生徒、教師、クラスを管理するためのエンドポイントを提供します。

## Project Structure / プロジェクト構造

- `src/app`: Next.js app router pages / Next.jsアプリルーターページ
- `src/components`: Reusable UI components / 再利用可能なUIコンポーネント
- `src/lib`: Utility functions and API clients / ユーティリティ関数とAPIクライアント
- `public`: Static assets / 静的アセット

## Development / 開発

### Running the Development Server / 開発サーバーの実行

```bash
npm run dev
```

### Building for Production / 本番用ビルド

```bash
npm run build
```

### Running in Production Mode / 本番モードでの実行

```bash
npm start
```

## License / ライセンス

This project is licensed under the Apache License 2.0 - see the [LICENSE](../../LICENSE) file for details.

このプロジェクトは Apache License 2.0 の下でライセンスされています。詳細は [LICENSE](../../LICENSE) ファイルを参照してください。
