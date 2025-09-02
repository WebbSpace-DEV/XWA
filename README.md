# XWA
X-Wing Advisor

The "X-Wing Advisor" (XWA) web application demonstrates the capabilities of data visualizations, the responsiveness of AngularJS, and the application of best-practice information assurance and security.

Data is provided via RESTful web services using Minimal API and Vertical Slice Architecture in the orchestration layer. The application implements JSON Web Tokens (JWT) to authenticate and authorize access to the web API service endpoints.

In the data structure, the X-Wing fleet is organized by airframes, squadrons, and platforms. Airframes identify specific X-Wing vehicles, squadrons identify groups of X-Wing airframes, and platforms identify variations between X-Wing airframes.

Each X-Wing airframe has an identical set of five (5) provisioned components: "Sensor Window", "Astromech Droid", "Servo Actuator", "Power Generator", and "Deflector Shield". The provisions are linearly biased according to their criticality. In the module that generates "shim" data on the backend of the RESTful web service, each of the per-airframe provisions is randomly assigned a weighted score, and by applying the provision ordinal-bias business logic, an overall score is assigned to the airframe, with score averages aggregating forward through the fleet hierarchy. This allows the web application user to visualize and analyze the disposition of provisions, airframes, squadrons, platforms, airfields, and ultimately the readiness of fleet itself.

Level 3 ("red") scores range from 0 to 34, Level 2 ("amber") scores range from 35 to 49, and Level 1 ("green") scores range from 50 to 100.

XWA provides a framework for Descriptive and Diagnostic analytics, and Prescriptive analytics inferences might be made across the fleet within its data structure hierarchy.

All of the visual elements in XWA are turn-key, solution-specific components written using the d3.js visualization library with AngularJS and JavaScript.
