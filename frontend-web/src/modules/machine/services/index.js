const apiBackend = "http://localhost:5004";

export const getMachine = async () => {
  return await fetch(`${apiBackend}/Maquina`).then((response) =>
    response.json()
  );
};

export const createMachine = async (data) => {
  return await fetch(`${apiBackend}/Maquina`, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify({ nome: data.machineName, marca: data.mark }),
  })
    .then((response) => response.json())
    .then((result) => {
      alert("Success:", result);
    })
    .catch((error) => {
      console.error("Error:", error);
    });
};

export const editMachine = async (id, { nome, marca }) => {
  return await fetch(`${apiBackend}/Maquina/${id}`, {
    method: "PUT",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify({ nome, marca }),
  })
    .then((response) => response.json())
    .catch((error) => {
      console.error("Error:", error);
    });
};

export const removeMachine = async (id) => {
  return await fetch(`${apiBackend}/Maquina/${id}`, {
    method: "DELETE",
    headers: {
      "Content-Type": "application/json",
    },
  })
    .then((response) => response.json())
    .then((data) => {
      alert("Success:", data);
    })
    .catch((error) => {
      console.error("Error:", error);
    });
};
