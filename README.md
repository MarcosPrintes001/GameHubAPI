
# GameHub API

API RESTful para gerenciamento de um catálogo de jogos, usuários, avaliações e favoritos, construída com .NET 7 e Entity Framework Core, usando SQLite como banco de dados.

## Funcionalidades

- CRUD completo para jogos, usuários, avaliações e favoritos
- Autenticação JWT para segurança nas operações
- Seed inicial para popular o banco com dados básicos
- Swagger integrado para testes e documentação da API

## Tecnologias

- .NET 7
- Entity Framework Core
- SQLite
- JWT (Json Web Token)
- Swagger/OpenAPI

## Como rodar

1. Clone o repositório  
   ```bash
   git clone https://github.com/seu-usuario/GameHub.API.git
   cd GameHub.API
   ```

2. Configure as variáveis de ambiente para JWT no `appsettings.json` ou `appsettings.Development.json`:

   ```json
   "Jwt": {
     "Key": "sua-chave-secreta-aqui",
     "Issuer": "GameHub",
     "Audience": "GameHubUser"
   }
   ```

3. Restaure os pacotes e rode as migrações para criar o banco:

   ```bash
   dotnet restore
   dotnet ef database update
   ```

4. Rode a aplicação:

   ```bash
   dotnet run
   ```

5. Acesse a documentação e teste os endpoints no Swagger:  
   [http://localhost:5120/swagger/index.html](http://localhost:5120/swagger/index.html)

## Endpoints principais

- `GET /api/games` - Lista todos os jogos  
- `POST /api/games` - Cria um novo jogo (requer token JWT)  
- `PUT /api/games/{id}` - Atualiza um jogo existente (requer token JWT)  
- `DELETE /api/games/{id}` - Remove um jogo (requer token JWT)  

- `GET /api/users` - Lista usuários (requer token JWT)  
- `POST /api/auth/register` - Registrar usuário  
- `POST /api/auth/login` - Login e obtenção do token JWT  

*(Endpoints de reviews e favoritos seguem padrão semelhante)*

## Como contribuir

1. Faça um fork do projeto  
2. Crie uma branch com sua feature (`git checkout -b minha-feature`)  
3. Faça commit das suas mudanças (`git commit -m 'feat: minha nova feature'`)  
4. Envie para a branch original (`git push origin minha-feature`)  
5. Abra um Pull Request  

## Autor

Marcos Printes - [github.com/MarcosPrintes001](https://github.com/MarcosPrintes001)
