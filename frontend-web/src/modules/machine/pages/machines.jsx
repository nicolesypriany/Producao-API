import Footer from "@/modules/core/components/footer";
import Header from "@/modules/core/components/header";
import { useCreateMachine } from "@/modules/machine/hooks/useCreateMachine";
import { Flex, TextInput } from "@mantine/core";
import TableMachines from "../components/Table";

const MachinesPage = () => {
  const { handleSubmit, onSubmit, errors, register } = useCreateMachine();

  return (
    // <form onSubmit={handleSubmit(onSubmit)}>
    <Flex width="100%" direction="column">
      <Header>Cadastro de máquinas</Header>

      <TableMachines />
    </Flex>
    // </form>
  );
};

export default MachinesPage;
