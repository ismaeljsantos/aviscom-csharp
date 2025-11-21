# Aviscom API

API RESTful construída em .NET 8 para o sistema Aviscom, focada em gestão de utilizadores (Pessoa Física e Jurídica), seus endereços, contactos e perfis associados.

## Principais Funcionalidades

- **CRUD de Utilizador Pessoa Física:** Gestão completa (Criar, Ler, Atualizar, Excluir) de utilizadores.
- **CRUD de Utilizador Pessoa Jurídica:** Gestão completa (Criar, Ler, Atualizar, Excluir) de utilizadores PJ.
- **Gestão de Perfil:** CRUDs completos para `Endereco`, `Contato`, `Escolaridade` e `ExperienciaProfissional`, associados tanto a Pessoas Físicas como Jurídicas.
- **Autenticação e Autorização (JWT):** Implementa login com JSON Web Tokens (JWT) para ambos PF e PJ, e autorização baseada em Funções (Roles) (ex: "Administrador").
- **Gestão de Funções (Admin):** CRUDs completos e seguros para gerir `Funcoes`, `Setores` e a sua associação aos utilizadores (`UsuarioFuncao`).
- **Segurança de Recurso:** Implementação da lógica de "Dono do Recurso" para todos os `UPDATE`s e `DELETE`s.
- **Segurança:**
  - **Hashing de Senha:** Senhas são armazenadas usando **BCrypt**.
  - **Hashing de Dados:** CPFs/CNPJs são hasheados com **SHA256** para verificação rápida de duplicidade.
  - **Criptografia:** CPFs são criptografados com **AES-256** (bidirecional) na base de dados.
- **Validação Avançada (DTOs):**
  - **CPF/CNPJ:** Aceita formatos com ou sem máscara.
  - **Datas:** Aceita múltiplos formatos de data (`ddMMyyyy`, `dd/MM/yyyy`, `dd-MM/yyyy`) na entrada.
  - **Validação Customizada:** Regras de negócio complexas (ex: validar `Valor` do contacto com base no `Tipo`).
- **Formatação de Resposta:**
  - **Datas:** Retorna datas no formato `dd/MM/yyyy`.
  - **Telefones:** Retorna números de telefone e celular com máscaras (ex: `(xx) xxxxx-xxxx`).
  - **IDs:** Utiliza `Ulid` para chaves primárias, retornados como strings.

## Tecnologias Utilizadas

- **.NET 8** (API)
- **Entity Framework Core 9** (ORM)
- **SQL Server** (Base de Dados)
- **Swashbuckle (Swagger)** (Documentação da API)
- **NUlid** (IDs únicos)
- **BCrypt.Net** (Hashing de Senhas)
- **Microsoft.AspNetCore.Authentication.JwtBearer** (Autenticação JWT)

## Como Executar (Setup)

1.  **Pré-requisitos:**

    - [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
    - Um servidor SQL Server (ex: SQL Express).

2.  **Clone o repositório:**

    ```bash
    git clone [https://github.com/ismaeljsantos/aviscom-csharp.git](https://github.com/ismaeljsantos/aviscom-csharp.git)
    cd aviscom-csharp/Aviscom
    ```

3.  **Configure as suas Conexões:**

    - Renomeie ou crie o ficheiro `appsettings.Development.json` na pasta `Aviscom/`.
    - Adicione as suas configurações. Ele deve parecer-se com isto:

    ```json
    {
      "ConnectionStrings": {
        "DefaultConnection": "Server=SEU_SERVIDOR;Database=AviscomDB;User Id=SEU_USUARIO;Password=SUA_SENHA;TrustServerCertificate=True;"
      },
      "EncryptionSettings": {
        "AESKey": "SUA_CHAVE_AES_256_EM_BASE64_AQUI"
      },
      "JwtSettings": {
        "SecretKey": "SUA_CHAVE_SECRETA_JWT_MUITO_LONGA_E_ALEATORIA_AQUI",
        "Issuer": "AviscomAPI",
        "Audience": "AviscomClient"
      }
    }
    ```

    - **Importante:** Gere chaves fortes e aleatórias para `AESKey` e `SecretKey`.

4.  **Aplique as Migrações:**

    - Execute o comando abaixo na pasta `Aviscom/` para criar a base de dados:

    ```bash
    dotnet ef database update
    ```

5.  **Rode a Aplicação:**

    ```bash
    dotnet run
    ```

6.  **Acesse a Documentação:**

    - Abra o seu navegador e acesse a URL do Swagger (geralmente `http://localhost:5172/swagger` ou `https://localhost:7196/swagger`, verifique o seu `launchSettings.json`).

7.  **Configurando o Primeiro Administrador:**
    - O _setup_ de administrador tem um problema de "ovo e a galinha" (precisa de ser admin para criar um admin).
    - **Solução Temporária:** Comente a linha `[Authorize(Policy = "Administrador")]` no topo de `FuncaoController.cs`, `SetorController.cs` e `UsuarioFuncaoController.cs`.
    - Execute a aplicação e use o Swagger (autenticado como um utilizador normal) para:
      1.  `POST /api/admin/funcoes` -> Criar a função `"Administrador"`.
      2.  `POST /api/admin/setores` -> Criar o setor `"Sistema"`.
      3.  `POST /api/admin/associacoes-funcao/pessoa-fisica` -> Associar o seu utilizador a essa função e setor.
    - **Finalmente:** Descomente as linhas `[Authorize(Policy = "Administrador")]` e reinicie a aplicação. O seu utilizador agora terá acesso total.

## Resumo da API (Endpoints)

### Autenticação (Login)

- `POST /api/auth/login/pessoa-fisica` - Autentica um utilizador PF e retorna um Token JWT.
- `POST /api/auth/login/pessoa-juridica` - Autentica um utilizador PJ e retorna um Token JWT.

### Utilizador Pessoa Física

- `POST /api/usuarios/pessoa-fisica` - **(Público)** Cria um novo utilizador.
- `GET /api/usuarios/pessoa-fisica` - **(Admin)** Lista todos os utilizadores.
- `GET /api/usuarios/pessoa-fisica/{id}` - **(Logado)** Busca um utilizador por ID.
- `PATCH /api/usuarios/pessoa-fisica/{id}` - **(Logado/Dono)** Atualiza um utilizador (parcial).
- `DELETE /api/usuarios/pessoa-fisica/{id}` - **(Admin)** Exclui um utilizador.

### Utilizador Pessoa Jurídica

- `POST /api/usuarios/pessoa-juridica` - **(Público)** Cria um novo utilizador.
- `GET /api/usuarios/pessoa-juridica` - **(Admin)** Lista todos os utilizadores.
- `GET /api/usuarios/pessoa-juridica/{id}` - **(Logado)** Busca um utilizador por ID.
- `PATCH /api/usuarios/pessoa-juridica/{id}` - **(Logado/Dono)** Atualiza um utilizador (parcial).
- `DELETE /api/usuarios/pessoa-juridica/{id}` - **(Admin)** Exclui um utilizador.

### Endereços (Requer Login)

- `POST /api/usuarios/pessoa-fisica/{usuarioPfId}/enderecos` - Cria um endereço para um PF.
- `GET /api/usuarios/pessoa-fisica/{usuarioPfId}/enderecos` - Lista os endereços de um PF.
- `POST /api/usuarios/pessoa-juridica/{usuarioPjId}/enderecos` - Cria um endereço para um PJ.
- `GET /api/usuarios/pessoa-juridica/{usuarioPjId}/enderecos` - Lista os endereços de um PJ.
- `GET /api/enderecos/{id}` - **(Logado/Dono)** Busca um endereço por seu ID.
- `PUT /api/enderecos/{id}` - **(Logado/Dono)** Atualiza um endereço por seu ID.
- `DELETE /api/enderecos/{id}` - **(Logado/Dono)** Exclui um endereço por seu ID.

### Contactos (Requer Login)

- `POST /api/usuarios/pessoa-fisica/{usuarioPfId}/contatos` - Cria um contacto para um PF.
- `GET /api/usuarios/pessoa-fisica/{usuarioPfId}/contatos` - Lista os contactos de um PF.
- `POST /api/usuarios/pessoa-juridica/{usuarioPjId}/contatos` - Cria um contacto para um PJ.
- `GET /api/usuarios/pessoa-juridica/{usuarioPjId}/contatos` - Lista os contactos de um PJ.
- `GET /api/contatos/{id}` - **(Logado/Dono)** Busca um contacto por seu ID.
- `PUT /api/contatos/{id}` - **(Logado/Dono)** Atualiza um contacto por seu ID.
- `DELETE /api/contatos/{id}` - **(Logado/Dono)** Exclui um contacto por seu ID.

### Escolaridade (Requer Login)

- `POST /api/usuarios/pessoa-fisica/{usuarioPfId}/escolaridades` - Cria um registo de escolaridade.
- `GET /api/usuarios/pessoa-fisica/{usuarioPfId}/escolaridades` - Lista as escolaridades de um utilizador.
- `GET /api/escolaridades/{id}` - **(Logado/Dono)** Busca uma escolaridade por seu ID.
- `PUT /api/escolaridades/{id}` - **(Logado/Dono)** Atualiza uma escolaridade por seu ID.
- `DELETE /api/escolaridades/{id}` - **(Logado/Dono)** Exclui uma escolaridade por seu ID.

### Experiência Profissional (Requer Login)

- `POST /api/usuarios/pessoa-fisica/{usuarioPfId}/experiencias` - Cria um registo de experiência.
- `GET /api/usuarios/pessoa-fisica/{usuarioPfId}/experiencias` - Lista as experiências de um utilizador.
- `GET /api/experiencias/{id}` - **(Logado/Dono)** Busca uma experiência por seu ID.
- `PUT /api/experiencias/{id}` - **(Logado/Dono)** Atualiza uma experiência por seu ID.
- `DELETE /api/experiencias/{id}` - **(Logado/Dono)** Exclui uma experiência por seu ID.

### Administração (Requer Admin)

- `GET /api/admin/funcoes` - Lista todas as Funções (Roles).
- `POST /api/admin/funcoes` - Cria uma nova Função.
- `...` (CRUD completo para Funções)
- `GET /api/admin/setores` - Lista todos os Setores.
- `POST /api/admin/setores` - Cria um novo Setor.
- `...` (CRUD completo para Setores)
- `POST /api/admin/associacoes-funcao/pessoa-fisica` - Associa um utilizador PF a uma Função/Setor.
- `DELETE /api/admin/associacoes-funcao/pessoa-fisica` - Remove uma associação.
- `GET /api/admin/associacoes-funcao/pessoa-fisica/{usuarioPfId}` - Lista as associações de um utilizador.
- `POST /api/admin/associacoes-funcao/pessoa-juridica` - Associa um utilizador PJ a uma Função/Setor.
- `DELETE /api/admin/associacoes-funcao/pessoa-juridica` - Remove uma associação.
- `GET /api/admin/associacoes-funcao/pessoa-juridica/{usuarioPjId}` - Lista as associações de um utilizador.
- `GET /api/empresas` - Lista todas as Empresas.
- `POST /api/empresas` - Cria uma nova Empresa.
- `...` (CRUD completo para Empresas)
- `GET /api/instituicoes` - Lista todas as Instituições.
- `POST /api/instituicoes` - Cria uma nova Instituição.
- `...` (CRUD completo para Instituições)
