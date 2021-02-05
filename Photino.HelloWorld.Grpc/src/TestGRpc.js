const {HelloRequest, HelloReply} = require('./greet_pb.js');
const {GreeterClient} = require('./greet_grpc_web_pb.js');

function SayHello(name) {
    var client = new GreeterClient('http://localhost:5000');

    var request = new HelloRequest();
    request.setName(name);

    client.sayHello(request, {}, (err, response) => {
        alert(response.getMessage());
    });
}

module.exports = SayHello;
