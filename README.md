# infludash-api

The API powering Infludash

## TODO

### General

- [ ] Create single response format

### Authentication / Authorization

- [x] Login flow
- [x] Register flow
- [x] Bcrypt implementation
- [ ] Make general environments file for storing secrets (probably in json)
- [x] Implement JWT in current authentication
- [x] Authorize all routes except login and register
- [ ] HIBP API implementation for password
- [ ] Implement refreshTokens
- [ ] Add logout route that deletes refreshToken
