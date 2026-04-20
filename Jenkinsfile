pipeline {
    agent any
    agent any

    stages {
        stage('1. Build') {
            steps {
                echo 'Starting the Build Stage...'
                // Restores dependencies and builds the .NET project
                sh 'dotnet restore'
                sh 'dotnet build --configuration Release --no-restore'
            }
        }
    }
}
