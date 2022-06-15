# GitHub / Azure App Service CI/CD Configuration
Web applications will be configured to deploy to three default Azure App Service Slots (main,dev,uat).  Each slot will run as an independant website, in an environment suited specifically for its purpose(production, user-testing, development). Application development in these three environments will be managed using GitHub version control, within three related branches(main-deployment-branch,uat-deployment-branch, dev-deployment-branch). Continuous Integration/Deployment is implemented via GitHub Actions, where a YAML script triggers code build and deploment automatically upon a ***push*** to a specific branch.  Use the following the steps, below, to configure this CI/CD automation.  Additional branches can configured as well, using the same pattern.  


## In Azure App Service

Create a slot for each environment
	Name the three default slots "main","dev" and "uat" for consistency
Configue and application environment settings and database connection strings to point to the correct resource in the application environment.

	

## In GitHub Repository
Create three GitHub branches for the three development environments.
	Name the branches "main-deployment-branch", "dev-deployment-branch" and uat-devployment-branch" for consistency.

Add GH Repository Setting Secrets for the Azure Publishing Profile of each slot
	Name the secrets "main-deployent-profile", "dev-deployment-profile", and "uat-deployment-profile" for cocsistency.
	Copy/Paste the Publish-Profile of each Azure App Service Slot as the secret value. 

Create a YAML file for each Azure Deployment Slot
	Each Yaml file should be triggerd by pushes from the specific branch
	Each Yaml file should checkout the specific branch.
	Each Yaml file should compile and build the website files from that branch
	Each Yaml file should publish to the specific slot.
	Each Yaml file should point to a publish profile via the specific secret created in the prior step.


## Test

Merge the yaml files into the branches 
Push a minor revision into each branch to trigger the CI/CD action.
Verify the deployment

 

 