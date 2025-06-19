📡 FakeStore API - Backend (.NET 9 + DDD + SOLID)
Descrição
Este é o backend da aplicação FakeStore CRUD, desenvolvido em .NET 9 Web API, com arquitetura DDD (Domain-Driven Design) e aplicação de princípios SOLID.

As imagens dos produtos são enviadas via **multipart/form-data** e armazenadas em um **Azure Blob Storage**, evitando peso no banco de dados.

Variaveis de conexão para que for testar - Estará disponível por tempo limitado!
DEFAULT_CONNECTION=Server=tcp:srv-marchioro.database.windows.net,1433;Initial Catalog=testeFakeStore;User ID=Marchioro;Password=Cri$1984;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;
AZURE_BLOB_STORAGE=DefaultEndpointsProtocol=https;AccountName=mrcfakestore;AccountKey=kNehbEWTmoeB4BH4g3VtaC4hrgEKizkRwYz/+sjgniVxcjdl8WvInH+L5sCt/w9xUIzmuVRUfB9h+ASt6pp3YQ==;EndpointSuffix=core.windows.net

O Banco de Dados é em Núvem - SQL Server Online (Cloud Hosted)
![image](https://github.com/user-attachments/assets/b2183b08-f2f0-40f5-bd7e-4581aa0a72a5)


Tecnologias Utilizadas
✅ .NET 9 Web API

✅ Entity Framework Core

✅ SQL Server Online (Cloud Hosted)

✅ Azure Blob Storage (Armazenamento de imagens)

✅ FluentValidation (Validação de dados)

✅ Swagger / OpenAPI (Swashbuckle) (Documentação da API)

✅ DDD - Domain Driven Design

✅ Princípios SOLID

✅ Arquitetura em camadas: Domain, Application, Infrastructure, Communication.

Funcionalidades da API
✅ CRUD completo de produtos

✅ Upload de imagem com armazenamento externo no Azure Blob, retornando URL pública

✅ Filtros por Nome e Código de Barras com paginação e ordenação

✅ Ordenação por campos como ID e Preço

✅ Paginação eficiente com total de registros

✅ Validação de regras de negócio antes de persistência no banco

✅ Controle de erros e mensagens padronizadas

| **Método HTTP** | **Endpoint**                        | **Descrição**                                                                                                                           |
| --------------- | ----------------------------------- | --------------------------------------------------------------------------------------------------------------------------------------- |
| **POST**        | `/api/Products/sync`                | Sincroniza os produtos da API externa FakeStore com o banco de dados local.                                                             |
| **GET**         | `/api/Products`                     | Retorna uma lista paginada de produtos com suporte a filtros por nome e código de barras, além de ordenação por campos como ID e preço. |
| **POST**        | `/api/Products/register-with-image` | Cadastra um novo produto, incluindo o upload de uma imagem via multipart/form-data. A imagem é armazenada no Azure Blob Storage.        |
| **DELETE**      | `/api/Products/{id}`                | Exclui um produto específico a partir do seu ID.                                                                                        |
| **PUT**         | `/api/Products/{id}`                | Atualiza os dados de um produto existente, com possibilidade de alteração de imagem.                                                    |



