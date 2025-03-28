import Header from "@/modules/core/components/header";
import { useCreateMachine } from "@/modules/machine/hooks/useCreateMachine";
import { Flex } from "@mantine/core";
import TableMachines from "../components/Table";
import { ModalAddItem } from "../components/add-machine";

const MachinesPage = () => {
  // const { handleSubmit, onSubmit, errors, register } = useCreateMachine();

  return (
    // <form onSubmit={handleSubmit(onSubmit)}>
    <Flex direction="column">
      <Flex>
        <Header>Cadastro de máquinas</Header>
        <ModalAddItem />
      </Flex>
      <TableMachines />
    </Flex>
    // </form>
  );
};

export default MachinesPage;
