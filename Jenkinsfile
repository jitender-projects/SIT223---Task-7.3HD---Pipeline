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
                // We securely inject the token you saved in Jenkins
                withCredentials([string(credentialsId: 'sonarcloud-token', variable: 'SONAR_TOKEN')]) {
                    // 1. Start the scanner
                    bat "dotnet sonarscanner begin /k:\jitender-projects_SIT223---Task-7.3HD---Pipeline\ /o:\jitender-projects /d:sonar.host.url=\"https://sonarcloud.io\" /d:sonar.token=\"%SONAR_TOKEN%\""
                    
                    // 2. Build the project again specifically for the scanner to watch
                    bat "dotnet build --no-incremental"
                    
                    // 3. End the scanner and push the report to SonarCloud
                    bat "dotnet sonarscanner end /d:sonar.token=\"%SONAR_TOKEN%\""
                }
            }
        }
    }
}
