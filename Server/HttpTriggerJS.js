module.exports = function (context, req) {
  context.log('JavaScript HTTP trigger function processed a request.');

  var playerId = req.query.playerId;

  context.bindings.outputDocument = context.bindings.inputDocument || { id: 'MyFavoriteDocument' };
  context.bindings.outputDocument[playerId] = context.bindings.outputDocument[playerId] || {};
  context.bindings.outputDocument[playerId].latitude = req.query.latitude;
  context.bindings.outputDocument[playerId].longitude = req.query.longitude;
  context.bindings.outputDocument[playerId].accuracy = req.query.accuracy;

  var opponentId = req.query.opponentId;

  context.done(null, {
    status: 200,
    body: opponentId ? context.bindings.outputDocument[opponentId] : context.bindings.outputDocument
  });
};
