# Dogtor

- [Repositorio Front-end Dogtor](https://github.com/joaoMarcos777/dogtor)
- [Link da aplicação](https://dogtor-one.vercel.app/home)
## Descrição
Este projeto é o back-end desenvolvido em C# utilizando Entity Framework Core (EFCore), Swagger para documentação de API e Azure SQL como banco de dados. Este projeto foi criado como parte do Projeto de Inovação Tecnologica (PIT) para conclusão de curso.

## Funcionalidades
- Cadastro de usuários (veterinários e clientes)
- Gerenciamento de pets
- Agendamento de consultas
- Histórico de consultas
- Autenticação e autorização de usuários

## Tecnologias Utilizadas
- C#: Linguagem de programação principal do projeto.
- Entity Framework Core (EFCore): ORM utilizado para interação com o banco de dados.
- Swagger: Ferramenta para documentação interativa da API.
- Azure SQL: Banco de dados em nuvem.

## Pré-requisitos
- .NET SDK 6.0+
- SQL Server
- Conta no Azure para configuração do Azure SQL

## Instalação e Configuração
1. Clone o repositório:
```
git clone https://github.com/seu-usuario/dogtor.git
cd dogtor
```
2. Configure o banco de dados:
Atualize a string de conexão no arquivo appsettings.json com suas credenciais do Azure SQL.
3. Aplicar migrações do EFCore:
```
dotnet ef database update
```
4. Execute o projeto:
```
dotnet run
```
5. Acesse a documentação da API:
Abra o navegador e acesse http://localhost:xxxx/swagger/index.html para visualizar e interagir com a documentação da API.
