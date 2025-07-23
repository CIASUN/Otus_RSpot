db = db.getSiblingDB('rspot_places');

// Организации с GUID
const org1 = {
  _id: "f9f8a9b2-ec9c-4bc0-a930-1b48b7c877b1",
  name: "Организация А",
  ownerUserId: "user-guid-0001"
};

const org2 = {
  _id: "bb9f6340-0d11-4b6c-8247-b4c35386d241",
  name: "Организация Б",
  ownerUserId: "user-guid-0002"
};

db.organizations.insertMany([org1, org2]);

// Рабочие места с привязкой по organizationId (тоже GUID в виде строк)
db.workspaces.insertMany([
  {
    _id: "18f3c1be-0f71-42c6-bd11-9137b2e4cf02",
    organizationId: org1._id,
    name: "Рабочее место 1",
    location: "Москва, Ленина 1",
    capacity: 8
  },
  {
    _id: "2e07f167-c0f2-4893-858f-4ef7c5e120ed",
    organizationId: org2._id,
    name: "Рабочее место 2",
    location: "СПб, Невский 45",
    capacity: 10
  }
]);
