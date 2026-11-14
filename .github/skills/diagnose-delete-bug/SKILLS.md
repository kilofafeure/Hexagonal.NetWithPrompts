---
name: diagnose-delete-bug
description: 'Diagnoses why a DELETE endpoint does not correctly remove a resource. Use it when the user reports that deleting an expense (or category) does not work, even though the request does not fail.'
---

# Diagnose DELETE Bug (DELETE)

Instead of following a fixed sequence, decide your next diagnostic step
based on what you observe in each response.

## Input data

- **Reported symptom**: description of the problem (e.g. "I delete the expense but it still shows up in the GET").
- **Affected entity**: `Expense`, `Category`, or another one with a `DELETE` endpoint.

## When to use

- The user reports that a `DELETE` does not remove the resource, even though no error is shown.

## Instructions

### 1. Reproduce the minimal flow

- tart the API: `dotnet run --project src/MyExpenses.ExpenseAPI`.
- CCreate the resource (e.g. `POST /expenses`) and capture the `id` from the response.
- Call `DELETE /expenses/{id}` with that id.
- Call `GET /expenses` to observe whether the resource is still present.

### 2. Decide the next step based on the result (diagnostic branch)

- **If the `DELETE` returned `404 Not Found`:**
  The problem is in the lookup before the deletion. Check the filter
  `FirstOrDefault(e => e.Id == id)` in `src/MyExpenses.ExpenseAPI/Program.cs`.
- **f the `DELETE` returned `204 No Content` but the `GET` still shows the resource:**
  The problem is in the repository. Check `RemoveAll(e => e.Id == entity.Id)` in `src/FileRepository/<Entity>Repository.cs`.
- **If the failure is intermittent:**
  Suspect a race condition. Check the use of `_lock`n the repository, and verify in `Program.cs` whether the repository is registered as `AddSingleton`(a single shared instance) instead of `AddTransient` o `AddScoped` (which would create distinct instances per request).

### 3. eport the root cause

- Indicate the exact file and line where the problem is.
- Explain the why, not just the "where".
