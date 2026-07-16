# Controle de Gastos Residenciais

Sistema web de controle de gastos residenciais desenvolvido como solução para um teste técnico de desenvolvimento full-stack. O projeto é composto por uma API REST em **.NET 8 (C#)** e um front-end moderno em **React (TypeScript)** com **Vite**.

---

## 🚀 Tecnologias Utilizadas

### Back-End
- **Plataforma**: .NET 8 (C#)
- **Acesso a Dados**: Entity Framework Core 8
- **Banco de Dados**: SQLite (persistência local simplificada)
- **Documentação**: Swagger / OpenAPI

### Front-End
- **Biblioteca**: React 19
- **Linguagem**: TypeScript
- **Ferramenta de Build**: Vite 8
- **Estilização**: CSS Vanilla (Design System com variáveis personalizadas)
- **Comunicação HTTP**: Axios
- **Iconografia**: Lucide React
- **Linter**: Oxlint

---

## 📋 Regras de Negócio & Requisitos Implementados

1. **Pessoa**:
   - Atributos: `Id` (chave primária auto-incremento), `Nome`, `Idade`.
   - Funcionalidades: Cadastro (Criação), Exclusão e Listagem.
   - **Exclusão em Cascata (Delete Cascade)**: Se uma pessoa for excluída, todas as suas transações vinculadas no banco de dados serão excluídas automaticamente por integridade referencial.

2. **Transação**:
   - Atributos: `Id` (chave primária auto-incremento), `Descrição`, `Valor`, `Tipo` (Receita ou Despesa), `PersonId` (referência obrigatória para Pessoa).
   - Funcionalidades: Lançamento (Criação) e Listagem.

3. **Restrição de Maioridade**:
   - **Regra**: Pessoas menores de 18 anos (`Idade < 18`) só estão autorizadas a registrar transações do tipo **Despesa**. Caso tentem registrar uma **Receita**, o sistema bloqueia e retorna um erro de negócio amigável (`HTTP 400 Bad Request`).

4. **Demonstrativo Financeiro e Consolidação de Totais**:
   - O painel exibe a listagem de todas as pessoas cadastradas contendo: o somatório das receitas daquela pessoa, o somatório de suas despesas e o saldo individual (`Receitas - Despesas`).
   - No rodapé/resumo do painel, exibe-se a soma global (acumulado geral de todas as receitas, despesas e saldo líquido da casa).

---

## 📁 Estrutura de Diretórios

```text
├── backend/
│   ├── Controllers/       # Controladores da API REST (Pessoas e Transações)
│   ├── Data/              # Contexto do EF Core (DbContext e Factory)
│   ├── DTOs/              # Data Transfer Objects para os relatórios financeiros
│   ├── Exceptions/        # Exceções personalizadas de regras de negócio
│   ├── Migrations/        # Histórico de migrações do banco de dados SQLite
│   ├── Models/            # Entidades do domínio (Pessoa, Transacao, TipoTransacao)
│   ├── Services/          # Interfaces e implementações das regras de negócio
│   ├── Program.cs         # Inicialização, DI e Middleware da API
│   └── TestRunner.cs      # Bateria de testes e verificações integradas locais
│
└── frontend/
    ├── src/
    │   ├── assets/        # Recursos de imagem e logotipos
    │   ├── services/      # Cliente HTTP Axios configurado e tipado
    │   ├── types/         # Definições de tipagem e interfaces TypeScript
    │   ├── App.tsx        # Componente SPA principal da interface
    │   ├── App.css        # Estilos específicos da interface premium
    │   ├── index.css      # Variáveis do Design System (HSL) e estilo global
    │   └── main.tsx       # Ponto de entrada do React
    ├── index.html         # Template HTML principal (carregamento de fontes)
    └── package.json       # Script de dependências e configuração do Node
```

---

## 🛠️ Como Executar o Projeto

### Pré-requisitos
- [.NET SDK 8](https://dotnet.microsoft.com/download/dotnet/8.0) instalado.
- [Node.js](https://nodejs.org/) (versão 20 ou superior) instalado.

---

### Passo 1: Inicializar e Rodar o Back-End

1. Navegue até a pasta do back-end:
   ```bash
   cd backend
   ```
2. Execute o comando para restaurar as dependências e iniciar o servidor de API:
   ```bash
   # Utilizando a CLI do .NET instalado (exemplo utilizando a rota do usuário)
   & "C:\Users\d7a8v\.dotnet\dotnet.exe" run
   ```
   A API iniciará no endereço padrão `http://localhost:5175`. Você pode acessar `http://localhost:5175/swagger` no seu navegador para testar os endpoints interativamente.

3. **(Opcional) Executar a bateria de testes e validação das regras de negócio**:
   No diretório `backend`, você pode rodar o comando especial para executar os testes em memória:
   ```bash
   & "C:\Users\d7a8v\.dotnet\dotnet.exe" run -- --test
   ```

---

### Passo 2: Inicializar e Rodar o Front-End

1. Abra outro terminal e navegue até a pasta do front-end:
   ```bash
   cd frontend
   ```
2. Instale as dependências de pacotes:
   ```bash
   npm install
   ```
3. Inicie o servidor de desenvolvimento do Vite:
   ```bash
   npm run dev
   ```
4. Abra o navegador no endereço indicado no console (normalmente `http://localhost:5173`).

---

## 🛡️ Decisões de Arquitetura e Boas Práticas

- **Princípio SOLID**: Lógica de banco de dados e validações foram desacopladas dos controladores, centralizadas em camadas de serviços (`PessoaService` e `TransacaoService`) que dependem de interfaces.
- **Tratamento de Exceções**: Exceções de negócio utilizam a classe customizada `BusinessException`. O `TransacoesController` captura essa exceção e mapeia o erro de volta ao cliente como um `Bad Request (400)` padronizado.
- **Estética Visual Premium**: Interface construída em *Sleek Dark Mode* moderno com micro-animações no clique/hover, fontes carregadas externamente (`Outfit`) e design flexível adaptável a resoluções de tela variadas.
