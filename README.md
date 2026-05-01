# DealChaser.ai

Full-stack web application that generates personalized Black Friday deal ideas using AI.

## Live Project
https://dealchaserai-dnh6bsffdmadawcf.germanywestcentral-01.azurewebsites.net/

## Features
- Users input preferences (budget, category, etc.)
- Backend generates realistic deal ideas using OpenAI
- Structured ASP.NET Core backend with dependency injection
- React frontend for interactive UI

## Tech Stack
- Backend: ASP.NET Core (.NET)
- Frontend: React + TypeScript
- AI: OpenAI API
- Deployment: Azure App Service

## Security
API keys are not stored in this repository.
They are managed via environment variables (Azure / local development).

## Setup

### Backend
- Configure API key in:
  - `appsettings.Development.json` (local)
  - or Azure environment variables

### Frontend
```bash
npm install
npm run dev
