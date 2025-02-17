def PROJECT_NAME = "Unity-Developer-Test---Asteroids"
def CUSTOM_WORKSPACE = "C:\\Users\\Osten Games\\Documents\\GitHub\\Unity-Developer-Test---Asteroids\\${PROJECT_NAME}"
def UNITY_VERSION = "2022.3.42f1"
def UNITY_INSTALLATION = "D:\\unity\\${UNITY_VERSION}\\Editor"
def UNITY_USERNAME = "marceloschulze@gmail.com"
def UNITY_PASSWORD = "Marcelo53"

pipeline {
    environment {
        PROJECT_PATH = "${CUSTOM_WORKSPACE}\\${PROJECT_NAME}"
        UNITY_LICENSE_PATH = "C:\\ProgramData\\Jenkins\\Unity\\Unity_lic.ulf"
    }

    agent {
        label {
            label ""
            customWorkspace "${CUSTOM_WORKSPACE}"
        }
    }

    stages {
        stage('Checkout') {
            steps {
                checkout scm
            }
        }

        stage('Activate Unity License') {
            steps {
                script {
                    withEnv(["UNITY_PATH=${UNITY_INSTALLATION}"]) {
                        bat '''
                        echo "Requesting Unity license with activation token..."
                        "%UNITY_PATH%/Unity.exe" -batchmode -logFile -quit -username "%UNITY_USERNAME%" -password "%UNITY_PASSWORD%" -nographics -returnlicense
                        '''
                    }
                }
            }
        }

        stage('Build Windows') {
            when {
                expression { BUILD_WINDOWS == 'true' }
            }
            steps {
                script {
                    withEnv(["UNITY_PATH=${UNITY_INSTALLATION}"]) {
                        bat '''
                        "%UNITY_PATH%/Unity.exe" -quit -batchmode -projectPath %PROJECT_PATH% -executeMethod BuildScript.BuildWindows -logFile -
                        '''
                    }
                }
            }
        }

        stage('Deploy Windows') {
            when {
                expression { DEPLOY_WINDOWS == 'true' }
            }
            steps {
                echo 'Deploy Windows'
            }
        }
    }

    post {
        always {
            cleanWs()
        }
    }
}