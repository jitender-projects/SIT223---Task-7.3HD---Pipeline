pipeline {
    agent any // Runs the pipeline on any available Jenkins agent

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
