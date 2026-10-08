# ADR-004: Modelar los convenios en clases aparte
* Estado: Aceptado
* Fecha: 2026-10-03
* Autores: Juliana Velandia

## Contexto

En el Legacy, el descuento por convenio dependía de `Paciente.TipoConvenio` (un `int`) y se resolvía con `if/else` dentro de `AgendarCita`: EPS paga el 30 % y Prepagada el 10 %. Cada convenio nuevo, por ejemplo con una aseguradora, obligaba a modificar el Gestor (viola OCP).

## Decisión

La interfaz `IConvenio` contiene el método `CalcularCostoConvenio()`, que devuelve la porción del copago que paga el paciente. Las implementaciones son `EPS` y `Prepagada`.

**Rationale:** los convenios cambian por razones comerciales, independientes de las especialidades y del flujo de citas. Aislarlos permite agregar uno nuevo sin modificar el código existente.

## Consecuencias

**Positivas**
* Un convenio nuevo se agrega con una clase nueva (OCP), lo que permite la extensibilidad.
* El cálculo del copago queda libre de condicionales por tipo de convenio.

**Negativas**
* Hay más clases pequeñas en el proyecto.
* Los porcentajes siguen escritos en el código. Cambiar uno requiere recompilar.

## Cumplimiento (Compliance)
* **Métricas:** la complejidad ciclomática de `CalculadoraCopago` no crece al agregar convenios.
