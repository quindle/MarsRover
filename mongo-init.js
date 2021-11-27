db.createUser(
        {
            user: "rover-user",
            pwd: "rover-password",
            roles: [
                {
                    role: "readWrite",
                    db: "roverdb"
                }
            ]
        }
);