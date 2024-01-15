const fetch = require('node-fetch');

module.exports = async function (context, req) {
    const response = await fetch('https://sbosscher-f.azurewebsites.net/api/incrementvaluefunction?code=f5gGUYkK_1kM9baSpu-m5T0FEpxHw2ELZUKjg3EABDg4AzFutBA18Q%3D%3D');
    const data = await response.text();
    context.res = { body: data };
};