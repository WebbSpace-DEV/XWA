# XWA
X-Wing Advisor

The "X-Wing Advisor" web application demonstrates many of the capabilities of AngularJS, data visualization, and best-practice information assurance and security.

Data is provided via RESTful web services using Minimal API and Vertical Slice Architecture in the orchestration layer. The application implements JSON Web Tokens (JWT) to authenticate and authorize access to the web API service endpoints.

In the sample data, the X-Wing fleet is organized by airframes, squadrons, and platforms. Airframes identify specific X-Wing vehicles, squadrons identify groups of X-Wing airframes, and platforms identify variations between X-Wing airframes.

Each X-Wing airframe has an identical set of five (5) provisioned components: "Sensor Window", "Servo Actuator", "Astromech Droid", "Power Generator", and "Deflector Shield". The provisions are linearly biased according to their criticality. In the module that generates "shim" data on the backend of the RESTful web service, each of the per-airframe provisions is randomly assigned a weighted score, and by applying the provision ordinal-bias business logic, an overall score is assigned to the airframe, with score averages aggregating forward through the fleet hierarchy. This allows the web application user to analyze and visualize the readiness of provisions, airframes, squadrons, platforms, airfields, and ultimately the readiness of fleet itself.

Level 3 ("red") scores range from 0 to 34, Level 2 ("amber") scores range from 35 to 49, and Level 1 ("green") scores range from 50 to 100.
