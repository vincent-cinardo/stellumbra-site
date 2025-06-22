window.auth = {
    loginAndReload: async function (loginData) {
        try {
            const response = await fetch("/api/Auth/Login", {
                method: "POST",
                credentials: "include",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify(loginData)
            });

            if (response.ok) {
                location.reload();
            } else {
                const errorText = await response.text();
                let error = JSON.parse(errorText);
                switch (error.errorCode) {
                    case "EmailNotConfirmed":
                        window.location.href = "/forum/resend-email-confirmation";
                        break;
                    case "InvalidUsername":
                    case "InvalidPassword":
                    case "LoginNotAllowed":
                    case "AccountLocked":
                        console.error("Login failed:", error.message);
                        alert("Login failed: " + error.message);
                        break;
                    default:
                        console.error("Unexpected login error:", error.message);
                        alert("Unexpected error: " + error.message);
                        break;
                }
            }
        } catch (e) {
            console.error("Network or server error", e);
        }
    },
    registerUser: async function (registrationData) {
    const response = await fetch('/api/Auth/Register', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({
            username: registrationData.username,
            email: registrationData.email,
            password: registrationData.password,
            confirmPassword: registrationData.confirmPassword
        })
    });

    if (response.ok) {
        location.reload();
    } else {
        const error = await response.text();
        console.error('Registration failed:', error);
        alert('Registration failed: ' + error);
    }
}
};