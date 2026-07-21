# Toma de decisiones

Este documento responde a las preguntas incluidas en la prueba técnica respecto al proceso de toma de decisiones.

---

## 1. Describe una decisión técnica reciente que hayas tomado con información incompleta. ¿Qué te faltaba y cómo decidiste avanzar?

En un proyecto reciente para el desarrollo de un CRM comercial, una de las primeras decisiones fue definir cómo cambiaría el estado comercial de una empresa conforme avanzara el proceso de ventas.

Inicialmente consideré que el sistema debía actualizar automáticamente ese estado al concluir determinadas actividades, como una visita, una reunión o una cotización. Sin embargo, en ese momento todavía no conocía completamente la forma en que el cliente gestionaba su proceso comercial.

En lugar de implementar esa lógica de inmediato, decidí validar el flujo con los usuarios del negocio antes de convertirlo en una regla del sistema.

Durante las sesiones de análisis descubrimos que el proceso comercial dependía en gran medida del criterio del vendedor y que dos oportunidades con actividades similares podían encontrarse en etapas completamente distintas. Automatizar el cambio de estado habría generado información incorrecta y limitado la flexibilidad que necesitaba el equipo comercial.

Con esa información, decidí que el estado comercial fuera una decisión explícita del usuario y no una consecuencia automática de las actividades registradas.

Esta experiencia reforzó la importancia de validar las reglas de negocio con los usuarios antes de automatizarlas y de no asumir que un proceso aparentemente lógico representa necesariamente la forma en que una organización trabaja.

---

## 2. Cuando dos enfoques técnicos parecen igual de válidos, ¿qué criterios usas para elegir uno?

Cuando dos alternativas son técnicamente viables, procuro dejar de lado las preferencias personales y evaluar cuál aporta mayor valor al proyecto.

Generalmente considero aspectos como:

- Que cumpla correctamente los requisitos del negocio.
- Que sea consistente con la arquitectura existente.
- Que facilite el mantenimiento futuro.
- Que sea fácil de comprender para otros desarrolladores.
- Que reduzca la complejidad innecesaria.
- Que pueda probarse de forma sencilla.
- Que permita evolucionar el sistema sin grandes cambios.

Si después de evaluar estos puntos ambas opciones siguen siendo equivalentes, normalmente elijo la solución más simple.

Con el tiempo he aprendido que una solución más sofisticada no necesariamente es una mejor solución. La simplicidad suele traducirse en menor costo de mantenimiento, facilita la incorporación de nuevos integrantes al equipo y reduce la probabilidad de introducir errores.

---

## 3. Cuenta un caso en el que hayas tenido que revertir o cambiar una decisión técnica propia. ¿Qué aprendiste de esa experiencia?

Durante la implementación de esta prueba técnica asumí inicialmente que la configuración de Docker Compose utilizando `depends_on` sería suficiente para que la API pudiera conectarse correctamente a SQL Server al iniciar los contenedores.

Al realizar las primeras pruebas observé que la API intentaba establecer la conexión antes de que el servicio de base de datos estuviera disponible, provocando errores durante el arranque.

Después de investigar el comportamiento de Docker Compose comprendí que la directiva `depends_on` únicamente garantiza el orden en que se inician los contenedores, pero no que el servicio contenido en ellos se encuentre listo para ser utilizado.

Con esa información decidí modificar la estrategia de inicio de la aplicación, incorporando un *entrypoint* que esperara a que el puerto de SQL Server estuviera disponible antes de iniciar la API. Esta solución hizo que el proceso de arranque fuera mucho más confiable y eliminó los errores ocasionados por el inicio prematuro de la aplicación.

Esta experiencia reforzó una lección importante para mí: comprender el funcionamiento de una herramienta implica conocer también sus limitaciones. Las pruebas de integración permitieron identificar una suposición incorrecta y ajustar la solución con base en el comportamiento real del entorno de ejecución.