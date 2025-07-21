db = db.getSiblingDB('rspot_places');

// 2 организации
const org1 = {
  _id: ObjectId(),
  name: "Организация А",
  ownerUserId: "user-guid-0001"
};

const org2 = {
  _id: ObjectId(),
  name: "Организация Б",
  ownerUserId: "user-guid-0002"
};

db.organizations.insertMany([org1, org2]);

// 2 места (по одному на каждую организацию)
db.workspaces.insertMany([
  {
    _id: ObjectId(),
    organizationId: org1._id,
    name: "Рабочее место 1",
    location: "Москва, Ленина 1",
    capacity: 8
  },
  {
    _id: ObjectId(),
    organizationId: org2._id,
    name: "Рабочее место 2",
    location: "СПб, Невский 45",
    capacity: 10
  }
]);
