# MyExpenses

Backend para registrar y consultar gastos personales, construido de forma incremental con .NET 10 y arquitectura hexagonal.

## Estado actual

La solución contiene proyectos .NET 10:

- `src/MyExpenses.Domain`: entidades y reglas de negocio del dominio.
- `src/MyExpenses.ApplicationLayer`: componente de aplicación de la arquitectura hexagonal. Contiene el puerto secundario genérico `IRepository<T>`, los puertos primarios asíncronos y sus casos de uso para registrar, consultar y eliminar gastos.
- `src/MyExpenses.FileRepository`: adaptador que implementa `IRepository<Expense>` con almacenamiento en memoria mediante una lista de gastos. Los datos no se conservan al cerrar la aplicación.
- `src/MyExpenses.ExpenseAPI`: Minimal API como punto de entrada HTTP. Expone `GET /` como comprobación de estado y endpoints para registrar, consultar y eliminar gastos.
- `Expense`: entidad de gasto con identificador `Guid`, categoría (`IdCategory`), concepto, cantidad, fecha, nota opcional, fecha de creación y fecha de borrado lógico.
- `DomainException`: excepción para comunicar errores de reglas de dominio a otras capas.

`IRepository<T>` define operaciones asíncronas genéricas para agregar, consultar y eliminar entidades. Los casos de uso de gastos reciben `IRepository<Expense>` por constructor, registran gastos mediante el dominio, devuelven únicamente gastos activos y eliminan gastos mediante borrado lógico. El proyecto de aplicación referencia al proyecto de dominio para utilizar la entidad `Expense`.

`MyExpenses.ExpenseAPI` tiene referencias a los proyectos de aplicación, repositorio y dominio. Registra `ExpenseRepository` como singleton para conservar los gastos en memoria entre peticiones, y registra los casos de uso con ciclo de vida scoped. Los endpoints están documentados y pueden probarse manualmente en `src/MyExpenses.ExpenseAPI/MyExpenses.ExpenseAPI.http`.

| Método | Ruta | Operación |
|---|---|---|
| `POST` | `/api/expenses` | Registra un gasto |
| `GET` | `/api/expenses` | Consulta los gastos activos |
| `GET` | `/api/expenses/{expenseId}` | Consulta un gasto activo por ID |
| `DELETE` | `/api/expenses/{expenseId}` | Elimina lógicamente un gasto |

En Development se publica el documento OpenAPI en `/openapi/v1.json`.

La entidad valida que el concepto no esté vacío, que `IdCategory` no sea un GUID vacío, que la cantidad sea mayor que cero y tenga como máximo dos decimales, y que la fecha sea válida. Los importes con más de dos decimales se rechazan; no se redondean automáticamente. El método `Delete()` marca el gasto como eliminado asignándole una fecha, sin borrarlo físicamente, y no permite marcarlo como eliminado más de una vez.

## Requisitos

- .NET 10 SDK

## Compilar

Desde la raíz del repositorio:

```powershell
dotnet build MyExpenses.sln
```

## Arquitectura

El proyecto sigue una arquitectura hexagonal con componentes separados para dominio, aplicación, persistencia y API. El dominio no incluye usuarios.

## Próximos incrementos

Las siguientes funcionalidades todavía no están implementadas:

1. **Calcular el total gastado**: añadir un caso de uso y un endpoint que sumen los importes de los gastos activos.
2. **Elegir e implementar persistencia duradera**: `ExpenseRepository` actualmente guarda los gastos en una `List<Expense>` en memoria, por lo que se pierden al reiniciar la API. Se definirá el almacenamiento duradero en un incremento posterior.
3. **Añadir pruebas automatizadas**: cubrir las reglas del dominio, los casos de uso, la persistencia y las respuestas de la API.

La interfaz gráfica no forma parte del backend actual; su alcance y tecnología quedan pendientes de especificación. Este README se actualizará conforme se implementen y definan los siguientes incrementos.
