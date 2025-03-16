import Footer from "@/modules/core/components/footer";
import Header from "@/modules/core/components/header";
import { useCreateMachine } from "@/modules/machine/hooks/useCreateMachine";
import { Flex, TextInput } from "@mantine/core";
import TableMachines from "../components/Table";

const MachinesPage = () => {
  const { handleSubmit, onSubmit, errors, register } = useCreateMachine();

  return (
    // <form>
    <Flex width="100%">
      <Flex p={4} direction="column" w="50%">
        <Header>Cadastro de máquinas</Header>

        <Flex direction="column" gap={4}>
          <TextInput
            label="Nome da máquina"
            placeholder="Digite o nome da máquina"
            {...register("machineName", {
              required: "Nome da máquina é obrigatório",
            })}
          />

          <TextInput
            label="Marca"
            placeholder="Digite o nome da marca"
            {...register("brand", {
              required: "Marca é obrigatório",
            })}
          />

          <TableMachines />

          {/* <Footer /> */}
        </Flex>
      </Flex>
    </Flex>
    // </form>
  );
};

export default MachinesPage;
