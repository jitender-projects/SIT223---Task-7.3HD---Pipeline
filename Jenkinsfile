pipeline {
    agent any

    stages {
        stage('1. Build') {
            steps {
                echo 'Starting the Build Stage...'
                bat 'dotnet restore'
                bat 'dotnet build --configuration Release --no-restore'
            }
        }
        stage('2. Test') {
            steps {
                echo 'Running automated tests...'
                bat 'dotnet test --configuration Release --no-build --verbosity normal'
            }
        }
        stage('3. Code Quality') {
            steps {
                echo 'Running SonarCloud Analysis...'
                bat 'dotnet tool restore'
                withCredentials([string(credentialsId: 'sonarcloud-token', variable: 'SONAR_TOKEN')]) {
                    bat 'dotnet sonarscanner begin /k:"jitender-projects_SIT223-7.3HD-Pipeline" /o:"jitender-projects" /d:sonar.host.url="https://sonarcloud.io" /d:sonar.token="%SONAR_TOKEN%"'
                    bat 'dotnet build --no-incremental'
                    bat 'dotnet sonarscanner end /d:sonar.token="%SONAR_TOKEN%"'
                }
            }
        }
        stage('4. Security') {
            steps {
                echo 'Running Security Scan on Dependencies...'
                bat 'dotnet list package --vulnerable --include-transitive'
            }
        }
        stage('5. Deploy') {
            steps {
                echo 'Deploying to Test/Staging Environment...'
                bat 'dotnet publish --configuration Release --no-build --output ./staging-environment'
            }
        }
        stage('6. Release') {
            steps {
                echo 'Promoting to Production Environment...'
                bat 'if not exist production-environment mkdir production-environment'
                bat 'powershell Compress-Archive -Path ./staging-environment/* -DestinationPath ./production-environment/release-build-%BUILD_NUMBER%.zip -Force'
            }
        }
        stage('7. Monitoring & Alerting') {
            steps {
                echo 'Sending deployment alert to Datadog...'
                withCredentials([string(credentialsId: 'datadog-api-key', variable: 'DD_API_KEY')]) {
                    // Uses curl to send a JSON event directly to your Datadog Event stream
                    bat 'curl -X POST "https://api.datadoghq.com/api/v1/events" -H "Content-Type: application/json" -H "DD-API-KEY: %DD_API_KEY%" -d "{\\"title\\": \\"Pipeline SIT223-7.3HD Alert\\", \\"text\\": \\"Release Build %BUILD_NUMBER% has been successfully deployed to the production environment and is now being monitored.\\", \\"tags\\": [\\"env:production\\", \\"app:robot-api\\"]}"'
                }
            }
        }
    }
}