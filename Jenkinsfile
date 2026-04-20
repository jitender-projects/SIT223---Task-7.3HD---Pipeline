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
                // Packages the application and all dependencies into a clean staging folder
                bat 'dotnet publish --configuration Release --no-build --output ./staging-environment'
                echo 'Successfully deployed to staging.'
            }
        }
    }
}