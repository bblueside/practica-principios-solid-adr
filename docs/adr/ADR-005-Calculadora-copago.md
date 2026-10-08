# ADR-005: Extraer el cálculo del copago a CalculadoraCopago

* Estado: Aceptado
* Fecha: 2026-10-05
* Autores: Juliana Velandia

## Contexto

En el Legacy, `AgendarCita` también calculaba la tarifa: costo base de 100, factor por especialidad, descuento por convenio, +20 si el paciente es de primera vez y +35 si requiere radiografía. Esto mezclaba tarificación con agendamiento, persistencia y notificación (viola SRP) y hacía que cualquier ajuste de tarifas tocara el flujo de citas.

## Decisión

Extraeremos la regla a `CalculadoraCopago`, detrás de la interfaz `ICalculadoraCopago`. Su método `CalcularCopago(IEspecialidad, IConvenio, esPrimeraVez, requiereRadiografia)` compone las estrategias de [ADR-003](ADR-003-Strategy-especialidades.md) y [ADR-004](ADR-004-Strategy-convenios.md):

`copago = 100 × factorEspecialidad × factorConvenio (+20 si es primera vez) (+35 si requiere radiografía)`

**Rationale:** la tarificación es una responsabilidad propia que cambia por decisiones financieras. Tenerla en una sola clase, detrás de una interfaz, permite reemplazar la fórmula sin tocar el agendamiento.

## Consecuencias

**Positivas**
* El Gestor ya no conoce reglas de tarifa (SRP), y el LCOM96b del Gestor baja de 0.50 a 0.00.
* La fórmula queda centralizada y es más fácil de leer y auditar.

**Negativas**
* Hay un tipo más (la interfaz y su implementación).
* Los recargos fijos (+20, +35) y el costo base siguen escritos en el código.

## Cumplimiento (Compliance)

* **Revisión de código en el PR:** ningún cálculo de copago puede vivir fuera de `ICalculadoraCopago`, ni en el Gestor ni en `Program`.
* **Métricas:** el LCOM96b de `GestorCitasOdontologicas` se mantiene en 0.00, y `CalculadoraCopago` no depende de infraestructura (su Ce solo incluye `IEspecialidad` e `IConvenio`).
