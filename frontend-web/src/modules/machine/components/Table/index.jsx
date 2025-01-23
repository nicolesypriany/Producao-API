import { useMachines } from "@/machine/hooks/useMachines";
import { Button, Flex, Heading, Input, Table } from "@chakra-ui/react";
import { useEffect } from "react";

const TableMachines = () => {
  const {
    machines,
    isEditingMachine,
    register,
    handleSubmit,
    handleGetMachines,
    handleEditMachine,
    handleRemoveMachine,
    handleSaveEditMachine,
    handleCancelEditMachine,
  } = useMachines();

  useEffect(() => {
    handleGetMachines();
  }, []);

  return (
    <Flex direction="column" gap={10}>
      <Flex w="100%" justifyContent="center">
        <Heading fontSize="4xl">Listagem de máquinas</Heading>
      </Flex>
      <form>
        {machines && machines.length > 0 && (
          <Table.Root w="100%">
            <Table.Header>
              <Table.Row>
                <Table.ColumnHeader>Id</Table.ColumnHeader>
                <Table.ColumnHeader>Nome</Table.ColumnHeader>
                <Table.ColumnHeader>Marca</Table.ColumnHeader>
                <Table.ColumnHeader textAlign="end">Ação</Table.ColumnHeader>
              </Table.Row>
            </Table.Header>
            <Table.Body>
              {machines.map((machine) => (
                <Table.Row key={machine.id}>
                  <Table.Cell>{machine.id}</Table.Cell>
                  {isEditingMachine === machine.id ? (
                    <>
                      <Table.Cell>
                        <Input
                          placeholder="Digite o nome da máquina"
                          {...register("machineName", {
                            required: "Nome da máquina é obrigatório",
                          })}
                        />
                      </Table.Cell>
                      <Table.Cell>
                        <Input
                          placeholder="Digite a marca da máquina"
                          {...register("markName", {
                            required: "Marca da máquina é obrigatória",
                          })}
                        />
                      </Table.Cell>
                    </>
                  ) : (
                    <>
                      <Table.Cell>{machine.nome}</Table.Cell>
                      <Table.Cell>{machine.marca}</Table.Cell>
                    </>
                  )}
                  <Table.Cell textAlign="end">
                    {isEditingMachine === machine.id ? (
                      <Button
                        onClick={handleSubmit((data) =>
                          handleSaveEditMachine(machine.id, data)
                        )}
                        bg="blue.500"
                        color="#fefefe"
                        mr={2}
                      >
                        Salvar
                      </Button>
                    ) : (
                      <Button
                        onClick={() => handleEditMachine(machine.id)}
                        bg="blue.500"
                        color="#fefefe"
                        mr={2}
                      >
                        Editar
                      </Button>
                    )}
                    {isEditingMachine === machine.id ? (
                      <Button
                        onClick={() => handleCancelEditMachine()}
                        bg="red.500"
                        color="#fefefe"
                      >
                        Cancelar
                      </Button>
                    ) : (
                      <Button
                        onClick={() => handleRemoveMachine(machine.id)}
                        bg="red.500"
                        color="#fefefe"
                      >
                        Remover
                      </Button>
                    )}
                  </Table.Cell>
                </Table.Row>
              ))}
            </Table.Body>
          </Table.Root>
        )}
      </form>
    </Flex>
  );
};

export default TableMachines;
