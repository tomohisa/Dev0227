# 学校管理システム

ASP.NET Core、Blazor、Next.js、およびSekibanイベントソーシングフレームワークを使用して構築された、学生、教師、クラスを管理するための包括的なWebアプリケーションです。

## 概要

学校管理システムは、教育機関の追跡と管理のための集中プラットフォームを提供します。管理者は学生、教師、クラスを管理し、これらのエンティティ間の関係を追跡することができます。

## 機能

- **学生管理**: 学生記録の登録、更新、削除
- **教師管理**: 教師記録の登録、更新、削除
- **クラス管理**: クラス記録の作成、更新、削除
- **関係管理**: 学生をクラスに割り当て、教師をクラスに割り当て
- **重複チェック**: 学生IDと教師IDの重複を防止
- **複数のフロントエンド**: BlazorとNext.jsの両方のフロントエンドが利用可能

## 技術スタック

- **バックエンド**: ASP.NET Core、Sekibanイベントソーシング、Orleans
- **フロントエンド**: Blazor、shadcn/uiコンポーネントを使用したNext.js
- **アーキテクチャ**: イベントソーシング、CQRS、ドメイン駆動設計
- **テスト**: ドメインテスト用のxUnit、エンドツーエンドテスト用のPlaywright

## プロジェクト構造

```
SchoolManagement/
├── SchoolManagement.Domain/           # ドメインモデル、イベント、コマンド
│   └── Aggregates/                    # ドメイン集約
│       ├── Classes/                   # クラス集約
│       ├── Students/                  # 学生集約
│       ├── Teachers/                  # 教師集約
│       └── WeatherForecasts/          # 天気予報集約
├── SchoolManagement.ApiService/       # APIエンドポイント
├── SchoolManagement.Web/              # Blazor Webフロントエンド
├── school-management-next/            # Next.jsフロントエンド
├── SchoolManagement.AppHost/          # サービスオーケストレーション用のAspireホスト
├── SchoolManagement.ServiceDefaults/  # 共通サービス設定
├── SchoolManagement.Playwright/       # エンドツーエンドテスト
└── SchoolManagement.Domain.Tests/     # ドメインユニットテスト
```

## アプリケーションの実行

Aspireホストでアプリケーションを実行するには：

```bash
cd SchoolManagement/SchoolManagement.AppHost
dotnet run --launch-profile https
```

Webフロントエンドへのアクセス：
- Blazorフロントエンド: https://localhost:7201
- APIサービス: https://localhost:7202

Next.jsフロントエンドを実行するには：

```bash
cd SchoolManagement/school-management-next
npm install
npm run dev
```

## ClineによるLLM開発

このリポジトリは、メモリバンクシステムを持つAIアシスタントであるClineと連携するように設計されています。メモリバンクには以下が含まれます：

- **projectbrief.md**: コア要件と目標
- **productContext.md**: このプロジェクトが存在する理由と解決する問題
- **systemPatterns.md**: システムアーキテクチャと設計パターン
- **techContext.md**: 使用されている技術と技術的制約
- **activeContext.md**: 現在の作業の焦点と最近の変更
- **progress.md**: 機能している部分と構築が残っている部分

Clineと作業する際は、**update memory bank**コマンドを使用して、Clineにすべてのメモリバンクファイルをレビューして更新させることができます。

## ライセンス

このプロジェクトはApache License 2.0の下でライセンスされています - 詳細は[LICENSE](./LICENSE)ファイルを参照してください。

[English README](./README.md)
