# ADR-009: Persistir el Id generado por el dominio al guardar una cita

* Estado: Aceptado
* Fecha: 2026-10-06
* Autores: Juliana Velandia

## Contexto

En el Legacy, la aplicación generaba el `Id` de la cita (`Guid` recortado a 8 caracteres), pero el `INSERT` no lo enviaba, así que la base de datos generaba otro con su `DEFAULT`. Después, `ActualizarCancelacion` buscaba la cita por el `Id` del dominio (`WHERE Id = @id`), que no existía en la BD. Resultado: la cancelación no actualizaba ninguna fila y no mostraba ningún error.

## Decisión

El `INSERT` de `SqlServerEjecutor.GuardarCita` enviará explícitamente la columna `Id`, con el valor generado por el dominio. El método de actualización se renombró a `ActualizarCita`, en línea con el contrato de `IRepositorio` ([ADR-001](ADR-001-Repositorio-inyectado.md)).


## Consecuencias

**Positivas**
* La cancelación ahora sí actualiza el registro correcto, lo que corrige la integridad de los datos.
* El Id del mensaje de confirmación coincide con el Id guardado en la BD, lo que mejora la trazabilidad.

**Negativas**
* El `DEFAULT` de `Citas.Id` en el esquema queda sin uso y podría ocultar un `INSERT` que olvide enviar el Id.

## Cumplimiento (Compliance)

* Todo `INSERT` sobre `Citas` incluye la columna `Id`, y toda consulta o actualización debe usar el Id del dominio.