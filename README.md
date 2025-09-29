# 🚚 VehicleChecklist API

API REST desenvolvida em **.NET 6** para gestão de **checklists de veículos** em uma empresa de transporte.  
O sistema permite que **executores** realizem checklists diários de veículos, e que **supervisores** ou **administradores** revisem e aprovem/reproem os registros.

---

## 🚀 Tecnologias utilizadas
- [.NET 8.0]
- [Entity Framework Core] (com SQL Server)
- [JWT Authentication](https://jwt.io/)
- [Swagger / OpenAPI](https://swagger.io/)
- [xUnit](https://xunit.net/) + [Moq](https://github.com/moq) (testes unitários)

---

## 📂 Estrutura do projeto

```
VehicleChecklist.sln
src/
  VehicleChecklist.Api/             # API (Controllers, Program.cs, Swagger)
  VehicleChecklist.Application/     # DTOs, Services (regras de negócio)
  VehicleChecklist.Domain/          # Entidades e Enums
  VehicleChecklist.Infrastructure/  # EF Core, Repositórios, DbContext
tests/
  VehicleChecklist.Tests/           # Testes unitários (xUnit + Moq)
```

---

## ⚙️ Configuração e execução

### 1. Clonar o repositório
```bash
git clone https://github.com/NoslenSilva/VehicleChecklist.git
cd VehicleChecklist
```

### 2. Subir banco de dados (SQL Server via Docker)

> Configuração ajustável em `appsettings.Development.json`.

### 3. Restaurar pacotes e aplicar migrations
```bash
dotnet restore
dotnet build
dotnet ef migrations add InitialCreate -p src/VehicleChecklist.Infrastructure -s src/VehicleChecklist.Api
dotnet ef database update -p src/VehicleChecklist.Infrastructure -s src/VehicleChecklist.Api
```

### 4. Executar a API
```bash
cd src/VehicleChecklist.Api
dotnet run
```

Acesse no navegador:  
👉 [https://localhost:5001/swagger](https://localhost:5001/swagger)

---

## 🔑 Autenticação

A aplicação utiliza **JWT Bearer Tokens**.

- Ao iniciar a API, um usuário **Admin padrão** é criado automaticamente (se não existir):

```json
{
  "email": "admin@teste.com",
  "password": "admin123"
}
```

### Fluxo:
1. **Login** → `POST /api/users/login`
   - Retorna o token JWT.
2. Enviar token no **Authorization Header**:
   ```
   Authorization: Bearer {seu-token}
   ```

---

## 📚 Endpoints principais

### 🔹 Users
- `POST /api/users/login` → Login (gera JWT)  
- `POST /api/users/register` → Criar usuário (somente Admin)  
- `GET /api/users` → Listar usuários (somente Admin)  

### 🔹 Vehicles
- `GET /api/vehicles` → Listar veículos  
- `GET /api/vehicles/{id}` → Buscar por Id  
- `GET /api/vehicles/by-plate/{plate}` → Buscar por placa  
- `POST /api/vehicles` → Criar (Admin, Supervisor)  
- `PUT /api/vehicles/{id}` → Atualizar (Admin, Supervisor)  
- `DELETE /api/vehicles/{id}` → Excluir (somente Admin)  

### 🔹 Checklists
- `POST /api/checklists/start` → Iniciar checklist (Executor/Admin)  
- `POST /api/checklists/start-by-plate` → Iniciar por placa (Executor/Admin)  
- `POST /api/checklists/add-item` → Adicionar item  
- `PUT /api/checklists/{id}/items/{itemId}` → Atualizar item  
- `DELETE /api/checklists/{id}/items/{itemId}` → Remover item  
- `POST /api/checklists/finish/{id}` → Finalizar checklist  
- `POST /api/checklists/review/{id}?approve={true|false}` → Revisar (Supervisor/Admin)  
- `GET /api/checklists/{id}` → Buscar checklist por Id  
- `GET /api/checklists` → Listar checklists (com filtros opcionais `vehicleId`, `executorId`, `status`)  

---

## 🧪 Testes

Rodar todos os testes unitários:

```bash
dotnet test
```

Exemplo de saída esperada:
```
Passed!  - Failed: 0, Passed: 12, Skipped: 0, Total: 12
```

## 👤 Autor
Desenvolvido por Noslen Ferreira da Silva para o processo seletivo .NET.  
