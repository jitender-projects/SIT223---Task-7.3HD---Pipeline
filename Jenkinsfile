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
                // 1. Create a production directory if it does not exist
                bat 'if not exist production-environment mkdir production-environment'
                
                // 2. Zip the staging files into a versioned release artifact using the Jenkins Build Number
                bat 'powershell Compress-Archive -Path ./staging-environment/* -DestinationPath ./production-environment/release-build-%BUILD_NUMBER%.zip -Force'
                
                echo 'Release artifact packaged and promoted to production successfully!'
            }
        }
    }
}