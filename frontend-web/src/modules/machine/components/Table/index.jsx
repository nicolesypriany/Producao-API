import { ActionIcon, Button, Flex, Table, TextInput } from "@mantine/core";
import { useEffect } from "react";
import { FaRegEdit, FaRegTrashAlt } from "react-icons/fa";
import { useGetMachines } from "../../hooks/useGetMachines";
import { useFormMachines } from "../../hooks/useMachines";

const TableMachines = () => {
  const { machines, handleGetMachines } = useGetMachines();
  const {
    editingMachineId,
    register,
    handleSubmit,
    handleEditMachine,
    handleRemoveMachine,
    handleSaveEditMachine,
    handleCancelEditMachine,
  } = useFormMachines();

  // TODO - Usar reactQuery para fazer a requisição e armazenar em cache e atualizar a lista
  // quando for removido algum item
  useEffect(() => {
    handleGetMachines();
  }, []);

  const rows = machines.map((machine) => (
    <Table.Tr key={machine.id}>
      {/* <form> */}
      <Table.Td>{machine.id}</Table.Td>
      {editingMachineId === machine.id ? (
        <>
          <Table.Td>
            <TextInput
              placeholder="Digite o nome da máquina"
              {...register("machineName", {
                required: "Nome da máquina é obrigatório",
              })}
            />
          </Table.Td>
          <Table.Td>
            <TextInput
              placeholder="Digite a marca da máquina"
              {...register("markName", {
                required: "Marca da máquina é obrigatória",
              })}
            />
          </Table.Td>
        </>
      ) : (
        <>
          <Table.Td>{machine.nome}</Table.Td>
          <Table.Td>{machine.marca}</Table.Td>
        </>
      )}
      <Table.Td>
        {editingMachineId === machine.id ? (
          <Button
            onClick={handleSubmit((data) =>
              handleSaveEditMachine(machine.id, data)
            )}
            mr={2}
          >
            Salvar
          </Button>
        ) : (
          <ActionIcon onClick={() => handleEditMachine(machine.id)}>
            <FaRegEdit />
          </ActionIcon>
        )}
      </Table.Td>
      <Table.Td>
        {editingMachineId === machine.id ? (
          <Button onClick={() => handleCancelEditMachine()}>Cancelar</Button>
        ) : (
          <ActionIcon onClick={() => handleRemoveMachine(machine.id)}>
            <FaRegTrashAlt />
          </ActionIcon>
        )}
      </Table.Td>
      {/* </form> */}
    </Table.Tr>
  ));

  return (
    <Flex direction="column">
      {machines && machines.length > 0 && (
        <Table>
          <Table.Thead>
            <Table.Tr>
              <Table.Th>Id</Table.Th>
              <Table.Th>Nome</Table.Th>
              <Table.Th>Marca</Table.Th>
              <Table.Th></Table.Th>
            </Table.Tr>
          </Table.Thead>
          <Table.Tbody>{rows}</Table.Tbody>
        </Table>
      )}
      {/* <Toaster /> */}
    </Flex>
  );
};

export default TableMachines;
