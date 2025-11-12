# Aviscom# Aviscom API

API RESTful construída em .NET 8 para o sistema Aviscom, focada em gerenciamento de usuários (Pessoa Física e Jurídica), seus endereços e contatos associados.

## Principais Funcionalidades

- **CRUD de Usuário Pessoa Física:** Gerenciamento completo (Criar, Ler, Atualizar, Excluir) de usuários.
- **CRUD de Entidades Associadas:** Gerenciamento de `Endereco` e `Contato` como recursos separados, ligados aos usuários.
- **Segurança:**
  - **Hashing de Senha:** Senhas são armazenadas usando **BCrypt**.
  - **Hashing de Dados:** CPFs são hasheados com **SHA256** para verificação rápida de duplicidade.
  - **Criptografia:** CPFs são criptografados com **AES-256** (bidirecional) no banco de dados.
- **Validação Avançada (DTOs):**
  - **CPF:** Aceita formatos com ou sem máscara (`12345678910` ou `123.456.789-10`).
  - **Datas:** Aceita múltiplos formatos de data (`ddMMyyyy`, `dd/MM/yyyy`, `dd-MM-yyyy`) na entrada.
  - **Validação Customizada:** Regras de negócio complexas (ex: validar `Valor` do contato com base no `Tipo`).
- **Formatação de Resposta:**
  - **Datas:** Retorna datas no formato `dd/MM/yyyy`.
  - **Telefones:** Retorna números de telefone e celular com máscaras (ex: `(xx) xxxxx-xxxx`).
  - **IDs:** Utiliza `Ulid` para chaves primárias, retornados como strings.

## Tecnologias Utilizadas

- **.NET 8** (API)
- **Entity Framework Core 9** (ORM)
- **SQL Server** (Banco de Dados)
- **Swashbuckle (Swagger)** (Documentação da API)
- **NUlid** (IDs únicos)
- **BCrypt.Net** (Hashing de Senhas)

## Como Executar (Setup)

1.  **Pré-requisitos:**

    - [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
    - Um servidor SQL Server (ex: SQL Express).

2.  **Clone o repositório:**

    ```bash
    git clone [https://github.com/ismaeljsantos/aviscom-csharp.git](https://github.com/ismaeljsantos/aviscom-csharp.git)
    cd aviscom-csharp/Aviscom
    ```

3.  **Configure suas Conexões:**

    - Renomeie ou crie o arquivo `appsettings.Development.json` na pasta `Aviscom/`.
    - Adicione suas configurações. Ele deve se parecer com isto:

    ```json
    {
      "ConnectionStrings": {
        "DefaultConnection": "Server=SEU_SERVIDOR;Database=AviscomDB;User Id=SEU_USUARIO;Password=SUA_SENHA;TrustServerCertificate=True;"
      },
      "EncryptionSettings": {
        "AESKey": "SUA_CHAVE_AES_256_EM_BASE64_AQUI"
      }
    }
    ```

    - **Importante:** Para gerar uma `AESKey` segura, você pode usar este trecho C# (no C# Interactive ou num projeto de console):
      ```csharp
      using var aes = System.Security.Cryptography.Aes.Create();
      Console.WriteLine(Convert.ToBase64String(aes.Key));
      ```

4.  **Aplique as Migrações:**

    - Execute o comando abaixo na pasta `Aviscom/` para criar o banco de dados:

    ```bash
    dotnet ef database update
    ```

5.  **Rode a Aplicação:**

    ```bash
    dotnet run
    ```

6.  **Acesse a Documentação:**
    - Abra seu navegador e acesse a URL do Swagger (geralmente `http://localhost:5172/swagger` ou `https://localhost:7196/swagger`, verifique seu `launchSettings.json`).

## Resumo da API (Endpoints)

### Usuário Pessoa Física

- `POST /api/usuarios/pessoa-fisica` - Cria um novo usuário.
- `GET /api/usuarios/pessoa-fisica` - Lista todos os usuários.
- `GET /api/usuarios/pessoa-fisica/{id}` - Busca um usuário por ID.
- `PATCH /api/usuarios/pessoa-fisica/{id}` - Atualiza um usuário (parcial).
- `DELETE /api/usuarios/pessoa-fisica/{id}` - Exclui um usuário.

### Endereços

- `POST /api/usuarios/pessoa-fisica/{usuarioPfId}/enderecos` - Cria um endereço para um usuário.
- `GET /api/usuarios/pessoa-fisica/{usuarioPfId}/enderecos` - Lista os endereços de um usuário.
- `GET /api/enderecos/{id}` - Busca um endereço por seu ID.
- `PUT /api/enderecos/{id}` - Atualiza um endereço por seu ID.
- `DELETE /api/enderecos/{id}` - Exclui um endereço por seu ID.

### Contatos

- `POST /api/usuarios/pessoa-fisica/{usuarioPfId}/contatos` - Cria um contato para um usuário.
- `GET /api/usuarios/pessoa-fisica/{usuarioPfId}/contatos` - Lista os contatos de um usuário.
- `GET /api/contatos/{id}` - Busca um contato por seu ID.
- `PUT /api/contatos/{id}` - Atualiza um contato por seu ID.
- `DELETE /api/contatos/{id}` - Exclui um contato por seu ID.
