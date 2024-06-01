exports.Request = {
    firebaseData: 'string',
}

exports.Response = {
}

exports.AdditionalUsings = [
    "using Firebase.Auth;",
]

exports.api = async function(firebaseData) {
    console.log(firebaseData);

    return {};
}