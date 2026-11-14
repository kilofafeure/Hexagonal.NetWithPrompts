---
name: http-endpoint-test
description: 'Whenever a new HTTP endpoint is created in the API, always add a corresponding test in the src/ExpenseAPI/requests.http file. Use it every time a route is added, modified, or registered in Program.cs'
---

# Add test in requests.http

Every time an endpoint is created, add a test request in
`src/MyExpenses.ExpenseAPI/MyExpenses.ExpenseAPI.http`.

## Input data

Before running this skill, make sure you have:

- **HTTP method**: GET, POST, PUT, DELETE, etc.
- **Endpoint route**: For example, `/expenses`, `/categories/{id}`
- **Content type** (optional): JSON, form-data, etc.
- **Example body** (if applicable): Sample data for POST/PUT

## When to use

- A route is created or modified in `src/MyExpenses.ExpenseAPI/Program.cs`.

## Instructions

1. Open `src/MyExpenses.ExpenseAPI/MyExpenses.ExpenseAPI.http`.
2. Add a block separated by `###` with the method, the URL (using `{{baseUrl}}`),
the headers, and the body if applicable

## Ejemplo

```http
### Create expense
POST {{baseUrl}}/expenses
Content-Type: application/json
Accept: application/json

{
  "concept": "Groceries",
  "amount": 23.45,
  "date": "2026-07-26",
  "idCategory": "11111111-1111-1111-1111-111111111111",
  "notes": "Supermarket"
}
```