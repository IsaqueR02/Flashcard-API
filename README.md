# Adapty-cards_API (Back-end)

[![.NET Core](https://img.shields.io/badge/.NET_Core-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/)
[![MySQL](https://img.shields.io/badge/MySQL-4479A1?style=for-the-badge&logo=mysql&logoColor=white)](https://www.mysql.com/)
[![Docker](https://img.shields.io/badge/Docker-2496ED?style=for-the-badge&logo=docker&logoColor=white)](https://www.docker.com/)

Este repositório contém o código-fonte da API RESTful do Adapty, o sistema que gerencia decks, flashcards, usuários e a lógica de repetição espaçada. Construído com ASP.NET Core, utilizando **MySQL** como banco de dados e Docker para orquestração.

## 📚 Sumário

1.  [Sobre o Projeto](#-sobre-o-projeto)
2.  [Tecnologias Utilizadas](#-tecnologias-utilizadas)
3.  [Pré-requisitos](#-pré-requisitos)
4.  [Configuração do Ambiente](#-configuração-do-ambiente)
5.  [Scripts Disponíveis](#-scripts-disponíveis)
6.  [Estrutura do Projeto](#-estrutura-do-projeto)
7.  [Rotas da API](#-rotas-da-api)
8.  [Contribuição](#-contribuição)
9. [Licença](#-licença)

## 💡 Sobre o Projeto

O Adapty é uma aplicação web **Mobile First** revolucionária focada em estudo com flashcards, **priorizando a acessibilidade e personalização para estudantes com divergências cognitivas**, como TDAH, dislexia e autismo. Nosso objetivo é promover a **inclusão e equidade (ODS 4)** por meio de aprendizado personalizado, valorizando a diversidade e garantindo **acesso igualitário a recursos educacionais (ODS 10)**.

O back-end é a espinha dorsal da aplicação, responsável por:

*   Gerenciamento de **Cadastro e Login** (RF001 - perfis seguros).
*   **CRUD de decks e cartões** (RF002, RF003).
*   **Comunicação:** Suporte para interação de dúvidas entre aluno e professor.
*   Implementação da lógica de **repetição espaçada** para uma **Progressão gradual e repetição espaçado** (RF004, RF005).
*   Armazenamento e processamento de dados para **Estatísticas de sessão** (RF009).
*   Suporte para **Exportação de Decks** em JSON/CSV (RF008).

## 💻 Tecnologias Utilizadas

*   **Framework:** ASP.NET Core
*   **Linguagem:** C#
*   **Banco de Dados:** MySQL
*   **Containerização:** Docker
*   **ORM:** Entity Framework Core (MySQL Provider)
*   **Autenticação:** JWT (JSON Web Tokens)
*   **Variáveis de Ambiente:** `appsettings.json` e variáveis de ambiente do sistema.

## ⚙️ Pré-requisitos

Antes de começar, certifique-se de ter instalado:

*   [.NET SDK (versão 8.x ou superior)](https://dotnet.microsoft.com/download)
*   [Docker Desktop](https://www.docker.com/products/docker-desktop/) (para rodar o banco de dados e a API em containers)
*   [Git](https://git-scm.com/downloads)

## 🚀 Configuração do Ambiente

Siga os passos abaixo para configurar e executar a API localmente usando Docker Compose:

1.  **Clone o Repositório:**
    ```bash
    git clone https://github.com/seu-usuario/Flashcard-API.git
    cd Flashcard-API
    ```

3.  **Construir e Iniciar os Serviços com Docker Compose:**
    ```bash
    docker-compose up --build
    ```
    *Este comando subirá o container do MySQL e da API. O Entity Framework Core aplicará as migrações automaticamente no startup. O Entity Framework Core aplicará as migrações automaticamente no startup, se configurado.*

4.  **Verificar a API:**
    A API estará rodando em `http://localhost:8080`.

## 📁 Estrutura do Projeto
adapty-backend/

├── src/

│ ├── Adapty.API/ # Projeto principal da API ASP.NET Core

│ │ ├── Controllers/ # Endpoints da API (Auth, Usuários(Aluno/Professor), Decks, Cartões)

│ │ ├── Models/ # DTOs, Request/Response Models

│ │ ├── Services/ # Lógica de negócio, Repetição Espaçada

│ │ ├── Data/ # Contexto do EF Core, Migrações

│ │ ├── Startup.cs # Configuração da aplicação (middleware, DI)

│ │ └── Program.cs # Ponto de entrada

│ ├── Adapty.Core/ # Biblioteca de classes compartilhadas (se houver)

│ └── Adapty.Tests/ # Projeto de testes (se separado)

├── docker-compose.yml # Definição dos serviços Docker (API, DB)

├── .env.example # Exemplo de variáveis de ambiente

├── .dockerignore # Arquivos a ignorar no build do Docker

├── .gitignore # Arquivos a ignorar no Git

└── Adapty.sln # Solução Visual Studio

## 📋 Rotas da API

A documentação completa das rotas da API (endpoints, métodos, parâmetros, exemplos de request/response) pode ser encontrada no [Swagger UI](http://localhost:8080/swagger) quando a API estiver rodando.

**Exemplos de Rotas:**

*   `POST /api/auth/register` - Cadastro de usuário
*   `POST /api/auth/login` - Autenticação e obtenção de JWT
*   `GET /api/decks` - Listar decks do usuário (requer autenticação)
*   `POST /api/decks` - Criar novo deck (requer autenticação)
*   `GET /api/decks/{deckId}/cards` - Listar cartões de um deck (requer autenticação)
*   `POST /api/decks/{deckId}/cards` - Adicionar novo cartão (requer autenticação)
*   `POST /api/study/session` - Iniciar sessão de estudo
*   `PUT /api/study/card/{cardId}/review` - Registrar revisão do cartão (Fácil, Difícil, Novamente)
